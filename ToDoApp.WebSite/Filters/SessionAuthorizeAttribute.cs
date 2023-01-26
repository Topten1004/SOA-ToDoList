using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ToDoApp.WebSite.Filters
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["usrid"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            FormsAuthentication.SignOut();
            filterContext.Result = new RedirectResult("/account/login");
        }
    }
}