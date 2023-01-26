using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using ToDoApp.ApiTest.Helper;

namespace ToDoApp.ApiTest
{
    [TestClass]
    public class TestControllerUnitTest : ApiTestBase
    {
        [TestMethod]
        public void Get()
        {
            var request = new RestRequest("tests", Method.GET);
            AddAuthHeaders(ref request, "get", "tests");

            IRestResponse response = RestClient.Execute(request);
            var content = response.Content;

        }
    }
}
