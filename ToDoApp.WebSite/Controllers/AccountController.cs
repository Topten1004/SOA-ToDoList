using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using RestSharp;
using ToDoApp.Contract;
using ToDoApp.Resources;
using ToDoApp.WebSite.Model;

namespace ToDoApp.WebSite.Controllers
{
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                return View();
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnurl)
        {
            if (ModelState.IsValid)
            {
                var request = new RestRequest("authentications", Method.GET);
                request.AddParameter("email", model.Email);
                request.AddParameter("password", model.Password);

                IRestResponse<int> response = RestClient.Execute<int>(request);
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Unauthorized)
                    ModelState.AddModelError("", ValidationMessages.Internal_Server_Error);
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                    ModelState.AddModelError("", ValidationMessages.Wrong_Password);
                else
                {
                    Session["usrid"] = response.Data;
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                return View();
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserContract model)
        {
            if (ModelState.IsValid)
            {
                var request = new RestRequest("authentications", Method.POST);
                request.AddJsonBody(model);

                IRestResponse<int> response = RestClient.Execute<int>(request);

                if (response.StatusCode != HttpStatusCode.OK)
                    ModelState.AddModelError("", ValidationMessages.Internal_Server_Error);
                else
                {
                    Session["usrid"] = response.Data;
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    HttpContext.User.Identity.GetUserId();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}