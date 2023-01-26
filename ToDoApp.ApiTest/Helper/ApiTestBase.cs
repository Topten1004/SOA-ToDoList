using System;
using RestSharp;
using ToDoApp.Common.Authentication;

namespace ToDoApp.ApiTest.Helper
{
    public class ApiTestBase
    {
        public RestClient RestClient = new RestClient("http://localhost:1475/api");

        public void AddAuthHeaders(ref RestRequest restRequest, string httpMethod, string controller)
        {
            var dateString = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var headerString = string.Format("{0}\n{1}\n/api/{2}\n", httpMethod.ToUpper(), dateString, controller);
            var hashedSignature = HmacUtility.ComputeHash("12345678", headerString);

            restRequest.AddHeader("Timestamp", dateString);
            restRequest.AddHeader("Authentication", "test@test.com:" + hashedSignature);
        }
    }
}
