using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ToDoApp.Common.Authentication;
using ToDoApp.Core.Persistence;

namespace ToDoApp.WebApi.Filters
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        public IUserRepository Repository { get; set; }

        private bool IsAuthenticated(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;

            var timeStampString = HmacUtility.GetHttpRequestHeader(headers, HmacUtility.TimestampHeaderName);
            if (!HmacUtility.IsDateValidated(timeStampString))
                return false;

            var authenticationString = HmacUtility.GetHttpRequestHeader(headers, HmacUtility.AuthenticationHeaderName);
            if (string.IsNullOrEmpty(authenticationString))
                return false;

            var authenticationParts = authenticationString.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (authenticationParts.Length != 2)
                return false;

            var username = authenticationParts[0];
            var signature = authenticationParts[1];

            if (!HmacUtility.IsSignatureValidated(signature))
                return false;

            HmacUtility.AddToMemoryCache(signature);

            var hashedPassword = username;
            var baseString = HmacUtility.BuildBaseString(actionContext);

            return HmacUtility.IsAuthenticated(hashedPassword, baseString, signature);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var isAuthenticated = IsAuthenticated(actionContext);

            if (!isAuthenticated)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response = response;
            }
        }
    }
}
