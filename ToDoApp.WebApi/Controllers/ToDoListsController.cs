using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Web.Http;
using ExpressMapper;
using ToDoApp.Contract;
using ToDoApp.Core.Persistence;
using ToDoApp.Core.Service.AppService;
using ToDoApp.Entity.Model;
using ToDoApp.WebApi.Filters;
using ToDoApp.WebApi.Utility;

namespace ToDoApp.WebApi.Controllers
{
    //[Authenticate]
    [RoutePrefix("api/todolists")]
    public class ToDoListsController : ApiController
    {
        private readonly ToDoListService _toDoListService;
        public ToDoListsController()
        {
            var unitOfWork = IoCUtility.Resolve<IUnitOfWork<DbContext>>();
            var repositoryFactory = IoCUtility.Resolve<IRepositoryFactory>();

            _toDoListService = new ToDoListService(unitOfWork, repositoryFactory);
        }

        [HttpGet]
        public IHttpActionResult Get(int userId)
        {
            var entity = _toDoListService.GetAll(userId);
            var viewModel = Mapper.Map<List<ToDoList>, List<ToDoListContract>>(entity);

            return Json(viewModel);
        }

        [HttpPost]
        public IHttpActionResult Post(ToDoListContract toDoListContract)
        {
            var mappedEntity = Mapper.Map<ToDoListContract, ToDoList>(toDoListContract);
            var savedEntity = _toDoListService.Save(mappedEntity);

            if (savedEntity != null)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        public IHttpActionResult Put(ToDoListContract toDoListContract)
        {
            var mappedEntity = Mapper.Map<ToDoListContract, ToDoList>(toDoListContract);
            var updatedEntity = _toDoListService.Save(mappedEntity);

            if (updatedEntity != null)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var status = _toDoListService.Delete(id);

            if (status)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}
