using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using Newtonsoft.Json;
using ToDoApp.WebApi.Helper;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

namespace ToDoApp.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        private BackgroundJobServer _backgroundJobServer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ConfigureFormatter(GlobalConfiguration.Configuration);
            ContractMapping.MappingRegistration();

            GlobalInitializer.Initialize();

            _backgroundJobServer = new BackgroundJobServer(
                new BackgroundJobServerOptions()
                {
                    Queues = new string[] { "default", "checker" },
                    ServerName = Environment.MachineName
                });
        }

        private void ConfigureFormatter(HttpConfiguration configuration)
        {
            var formatters = configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            var jsonFormatter = configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Reuse,
                TypeNameHandling = TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _backgroundJobServer.Dispose();
        }
    }
}
