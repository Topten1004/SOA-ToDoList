using System.Collections.Generic;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Core.Persistence
{
    public interface IUserRepository: IRepository<User, int>
    {
        User GetUserByEmail(string email);
        IList<User> Search(UserSearchArgs args);
    }
}
