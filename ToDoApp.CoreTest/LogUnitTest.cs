using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoApp.Core.Service.Log;
using ToDoApp.Entity.Model;
using ToDoApp.Test.Helper;

namespace ToDoApp.Test
{
    [TestClass]
    public class LogUnitTest
    {
        private readonly IAuditLogService _auditLogService;

        public LogUnitTest()
        {
            _auditLogService = IoCUtility.Resolve<IAuditLogService>();
        }

        [TestMethod]
        public void SaveLog()
        {
            _auditLogService.Save("UnitTest", new AuditLogItem<int, object>
            {
                Action = "Insert",
                Timestamp = DateTime.Now,
                Entity = new
                {
                    Name = "Hasan",
                    Surname = "Gedik"
                },
                Message = "Success"
            });
        }
    }
}
