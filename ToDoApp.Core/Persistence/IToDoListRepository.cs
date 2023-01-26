using System.Collections.Generic;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Core.Persistence
{
    public interface IToDoListRepository : IRepository<ToDoList, int>
    {
        IList<ToDoList> Search(ToDoListSearchArgs args);
        IList<ToDoList> GetNotificationNotSendItems();
        IList<ToDoList> GetAll(int userId);
    }
}
