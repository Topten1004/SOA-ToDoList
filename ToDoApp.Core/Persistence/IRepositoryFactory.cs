using System;

namespace ToDoApp.Core.Persistence
{
    public interface IRepositoryFactory : IDisposable
    {
        IUserRepository GetUserRepository();
        IToDoListRepository GeToDoListRepository();
        ITaskRepository GeTaskRepository();
    }
}
