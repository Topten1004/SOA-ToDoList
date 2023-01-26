using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace ToDoApp.Common.Authentication
{
    public static class HmacUtility
    {
        public const string AuthenticationHeaderName = "Authentication";
        public const string TimestampHeaderName = "Timestamp";

        public static string ComputeHash(string hashedPassword, string message)
        {
            var key = Encoding.UTF8.GetBytes(hashedPassword.ToUpper());
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }

        private static void AddNameValuesToCollection(List<KeyValuePair<string, string>> parameterCollection, NameValueCollection nameValueCollection)
        {
            if (!nameValueCollection.AllKeys.Any())
                return;

            foreach (var key in nameValueCollection.AllKeys)
            {
                var value = nameValueCollection[key];
                var pair = new KeyValuePair<string, string>(key, value);

                parameterCollection.Add(pair);
            }
        }

        private static List<KeyValuePair<string, string>> BuildParameterCollection(HttpActionContext actionContext)
        {
            var parameterCollection = new List<KeyValuePair<string, string>>();

            var queryStringCollection = actionContext.Request.RequestUri.ParseQueryString();
            var formCollection = HttpContext.Current.Request.Form;

            AddNameValuesToCollection(parameterCollection, queryStringCollection);
            AddNameValuesToCollection(parameterCollection, formCollection);

            return parameterCollection.OrderBy(pair => pair.Key).ToList();
        }

        private static string BuildParameterMessage(HttpActionContext actionContext)
        {
            var parameterCollection = BuildParameterCollection(actionContext);
            if (!parameterCollection.Any())
                return string.Empty;

            var keyValueStrings = parameterCollection.Select(pair => $"{pair.Key}={pair.Value}");

            return string.Join("&", keyValueStrings);
        }

        public static string GetHttpRequestHeader(HttpHeaders headers, string headerName)
        {
            if (!headers.Contains(headerName))
                return string.Empty;

            return headers.GetValues(headerName).SingleOrDefault();
        }

        public static string BuildBaseString(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            string date = GetHttpRequestHeader(headers, TimestampHeaderName);

            string methodType = actionContext.Request.Method.Method;

            var absolutePath = actionContext.Request.RequestUri.AbsolutePath.ToLower();
            var uri = HttpContext.Current.Server.UrlDecode(absolutePath);

            string parameterMessage = string.Empty; //BuildParameterMessage(actionContext);
            string message = string.Join("\n", methodType, date, uri, parameterMessage);

            return message;
        }

        public static bool IsAuthenticated(string hashedPassword, string message, string signature)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                return false;

            var verifiedHash = ComputeHash(hashedPassword, message);
            if (signature != null && signature.Equals(verifiedHash))
                return true;

            return false;
        }

        public static bool IsDateValidated(string timestampString)
        {
            DateTime timestamp;

            bool isDateTime = DateTime.TryParseExact(timestampString, "yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AdjustToUniversal, out timestamp);

            if (!isDateTime)
                return false;

            var now = DateTime.UtcNow;
            if (timestamp < now.AddMinutes(-5))
                return false;

            if (timestamp > now.AddMinutes(5))
                return false;

            return true;
        }

        public static bool IsSignatureValidated(string signature)
        {
            var memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(signature))
                return false;

            return true;
        }

        public static void AddToMemoryCache(string signature)
        {
            var memoryCache = MemoryCache.Default;
            if (!memoryCache.Contains(signature))
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
                memoryCache.Add(signature, signature, expiration);
            }
        }

        
    }
}
