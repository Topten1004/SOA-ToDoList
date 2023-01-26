using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using RestSharp;
using ToDoApp.Contract;

namespace ToDoApp.WebSite.Controllers
{
    public class TaskController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public JsonResult Get(int toDoListId)
        {
            var request = new RestRequest("tasks", Method.GET);
            AddAuthHeaders(ref request, HttpMethod.Get.Method, "tasks");
            request.AddParameter("toDoListId", toDoListId);

            IRestResponse<List<TaskContract>> response = RestClient.Execute<List<TaskContract>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Post(TaskContract model)
        {
            if (ModelState.IsValid)
            {
                var request = new RestRequest("tasks", Method.POST);
                AddAuthHeaders(ref request, HttpMethod.Post.Method, "tasks");
                request.AddJsonBody(model);

                IRestResponse response = RestClient.Execute(request);

                return response.StatusCode != HttpStatusCode.OK ? new HttpStatusCodeResult(HttpStatusCode.InternalServerError) : new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(TaskContract model)
        {
            if (ModelState.IsValid)
            {
                var request = new RestRequest("tasks", Method.PUT);
                AddAuthHeaders(ref request, HttpMethod.Put.Method, "tasks");
                model.ModifiedOn = DateTime.Now;
                request.AddJsonBody(model);

                IRestResponse response = RestClient.Execute(request);
                return response.StatusCode != HttpStatusCode.OK ? new HttpStatusCodeResult(HttpStatusCode.InternalServerError) : new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var request = new RestRequest("tasks", Method.DELETE);
            AddAuthHeaders(ref request, HttpMethod.Delete.Method, "tasks");
            request.AddParameter("id", id);

            IRestResponse response = RestClient.Execute(request);
            return response.StatusCode != HttpStatusCode.OK ? new HttpStatusCodeResult(HttpStatusCode.InternalServerError) : new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}