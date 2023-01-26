using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Web.Http;
using ExpressMapper;
using ToDoApp.Contract;
using ToDoApp.Core.Persistence;
using ToDoApp.Core.Service.AppService;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;
using ToDoApp.WebApi.Filters;
using ToDoApp.WebApi.Utility;

namespace ToDoApp.WebApi.Controllers
{
    //[Authenticate]
    [RoutePrefix("api/tasks")]
    public class TasksController : ApiController
    {
        private readonly TaskService _taskService;
        public TasksController()
        {
            var unitOfWork = IoCUtility.Resolve<IUnitOfWork<DbContext>>();
            var repositoryFactory = IoCUtility.Resolve<IRepositoryFactory>();

            _taskService = new TaskService(unitOfWork, repositoryFactory);
        }

        [HttpGet]
        public IHttpActionResult Get(int toDoListId)
        {
            var entity = _taskService.Search(new TaskSearchArgs
            {
                ToDoListId = toDoListId
            });
            var viewModel = Mapper.Map<IList<Task>, IList<TaskContract>>(entity);

            return Json(viewModel);
        }

        [HttpPost]
        public IHttpActionResult Post(TaskContract taskContract)
        {
            var mappedEntity = Mapper.Map<TaskContract, Task>(taskContract);
            var savedEntity = _taskService.Save(mappedEntity);

            if (savedEntity != null)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        public IHttpActionResult Put(TaskContract taskContract)
        {
            var mappedEntity = Mapper.Map<TaskContract, Task>(taskContract);
            var updatedEntity = _taskService.Save(mappedEntity);

            if (updatedEntity != null)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var status = _taskService.Delete(id);

            if (status)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}
