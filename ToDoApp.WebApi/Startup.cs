using Hangfire;
using Microsoft.Owin;
using Owin;
using ToDoApp.WebApi;

[assembly: OwinStartup(typeof(Startup))]

namespace ToDoApp.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #if DEBUG
            app.UseHangfireDashboard();
            #endif
        }
    }
}
