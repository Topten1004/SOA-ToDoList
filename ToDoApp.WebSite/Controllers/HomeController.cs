using System.Web.Mvc;
using ToDoApp.WebSite.Filters;

namespace ToDoApp.WebSite.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [SessionAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}