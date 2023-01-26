using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RestSharp;
using ToDoApp.Contract;

namespace ToDoApp.WebSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            ReportError(ex);
        }

        private void ReportError(Exception exception)
        {
            Task.Run(() =>
            {
                var baseClass = new Controllers.ControllerBase();
                var request = new RestRequest("auditlogs", Method.POST);
                baseClass.AddAuthHeaders(ref request, HttpMethod.Post.Method, "auditlogs");
                request.AddJsonBody(new AuditLogItemContract
                {
                    Action = Request.Url.PathAndQuery,
                    Message = exception.Message,
                    Entity = exception,
                });

                baseClass.RestClient.ExecuteAsync(request, restResponse => { });
            });
        }
    }
}
