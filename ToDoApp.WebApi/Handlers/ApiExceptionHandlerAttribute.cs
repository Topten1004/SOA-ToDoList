using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using ToDoApp.Core.Service.Log;
using ToDoApp.Entity.Model;
using ToDoApp.Resources;
using ToDoApp.WebApi.Utility;
using Task = System.Threading.Tasks.Task;

namespace ToDoApp.WebApi.Handlers
{
    public class ApiExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message);
            }
            else
            {
                ReportError(context);
#if DEBUG
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.Message);
#else
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ValidationMessages.Internal_Server_Error);
#endif
            }
        }

        private void ReportError(HttpActionExecutedContext actionExecutedContext)
        {
            Exception exception = actionExecutedContext.Exception;
            Task.Run(() =>
            {
                var auditLogService = IoCUtility.Resolve<IAuditLogService>();

                auditLogService.Save("ApiExceptionHandler", new AuditLogItem<int, object>
                {
                    Action = actionExecutedContext.Request.RequestUri.PathAndQuery,
                    Message = exception.Message,
                    Entity = exception
                });
            });
        }
    }
}