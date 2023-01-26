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
    [Authenticate]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly UserService _userService;
        public UsersController()
        {
            var unitOfWork = IoCUtility.Resolve<IUnitOfWork<DbContext>>();
            var repositoryFactory = IoCUtility.Resolve<IRepositoryFactory>();

            _userService = new UserService(unitOfWork, repositoryFactory);
        }

        [HttpGet]
        public IHttpActionResult Get(string email)
        {
            var entity = _userService.GetUserByEmail(email);
            var viewModel = Mapper.Map<User, UserContract>(entity);

            return Ok(viewModel);
        }

        [HttpPut]
        public IHttpActionResult Put(UserContract userContract)
        {
            var mappedEntity = Mapper.Map<UserContract, User>(userContract);
            var updatedEntity = _userService.Save(mappedEntity);

            if (updatedEntity != null)
                return Ok();

            return StatusCode(HttpStatusCode.InternalServerError);
        }
       
    }
}
