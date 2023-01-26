using System.Web.Http;
using ExpressMapper;
using ToDoApp.Contract;
using ToDoApp.Core.Service.Log;
using ToDoApp.Entity.Model;
using ToDoApp.WebApi.Utility;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.WebApi.Controllers
{
    [RoutePrefix("api/auditlogs")]
    public class AuditLogController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(AuditLogItemContract auditLogContract)
        {
            Task.Run(() =>
            {
                var auditLogService = IoCUtility.Resolve<IAuditLogService>();
                var mappedEntity = Mapper.Map<AuditLogItemContract, AuditLogItem<int, object>>(auditLogContract);

                auditLogService.Save("WebSiteExceptionHandler", mappedEntity);
            });
            return Ok();
        }
    }
}
