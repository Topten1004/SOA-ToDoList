using System.Collections.Generic;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Core.Service.Log
{
    public interface IAuditLogService
    {
        bool Save(string collectionName, AuditLogItem<int, object> item);
        bool Save(string collectionName, IEnumerable<AuditLogItem<int, object>> items);
        AuditLogItem<int, object> GetOne(string collectionName, AuditLogSearchArgs<int> searchArgs);
        IEnumerable<AuditLogItem<int, object>> Search(string collectionName, AuditLogSearchArgs<int> searchArgs);
    }
}
