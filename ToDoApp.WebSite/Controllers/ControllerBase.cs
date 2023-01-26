using System;
using System.Configuration;
using System.Web.Mvc;
using RestSharp;
using ToDoApp.Common.Authentication;

namespace ToDoApp.WebSite.Controllers
{
    public class ControllerBase : Controller
    {
        public RestClient RestClient = new RestClient(ConfigurationManager.AppSettings["ApiAddress"]);

        public void AddAuthHeaders(ref RestRequest restRequest, string httpMethod, string controller)
        {
            var dateString = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var headerString = string.Format("{0}\n{1}\n/api/{2}\n", httpMethod.ToUpper(), dateString, controller);
            var hashedSignature = HmacUtility.ComputeHash(HttpContext.User.Identity.Name, headerString);
            var authenticationHeaderString = string.Format("{0}:{1}", HttpContext.User.Identity.Name, hashedSignature);

            restRequest.AddHeader("Timestamp", dateString);
            restRequest.AddHeader("Authentication", authenticationHeaderString);
        }
    }
}