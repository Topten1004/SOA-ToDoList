using System.Collections.Generic;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Core.Persistence
{
    public interface ITaskRepository : IRepository<Task, int>
    {
        IList<Task> Search(TaskSearchArgs args);
        IList<Task> GetNotificationNotSendItems();
    }
}
