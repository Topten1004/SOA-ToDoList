using System.Data.Entity;
using System.Net;
using System.Web.Http;
using ExpressMapper;
using ToDoApp.Contract;
using ToDoApp.Core.Persistence;
using ToDoApp.Core.Service.AppService;
using ToDoApp.Entity.Model;
using ToDoApp.WebApi.Utility;

namespace ToDoApp.WebApi.Controllers
{
    [RoutePrefix("api/authentications")]
    public class AuthenticationsController : ApiController
    {
        private readonly UserService _userService;
        public AuthenticationsController()
        {
            var unitOfWork = IoCUtility.Resolve<IUnitOfWork<DbContext>>();
            var repositoryFactory = IoCUtility.Resolve<IRepositoryFactory>();

            _userService = new UserService(unitOfWork, repositoryFactory);
        }

        [HttpGet]
        public IHttpActionResult Get(string email, string password)
        {
            var entity = _userService.GetUserByEmail(email);
            if (entity == null)
                return NotFound();

            if(entity.Password == password)
                return Ok(entity.Id);

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public IHttpActionResult Post(UserContract userContract)
        {
            var mappedEntity = Mapper.Map<UserContract, User>(userContract);
            var savedEntity = _userService.Save(mappedEntity);

            if (savedEntity != null)
                return Ok(savedEntity.Id);

            return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}
