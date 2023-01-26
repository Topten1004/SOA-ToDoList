using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ToDoApp.Core.Persistence;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Infrastructure.Persistence
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        private readonly DbSet<User> _dbSet;
        public UserRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();

            _dbSet = unitOfWork.DatabaseContext.Set<User>();
        }

        public IList<User> Search(UserSearchArgs args)
        {
            if (args == null)
                return null;

            var result = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(args.Name))
                result = result.Where(x => x.Name.Contains(args.Name));

            if (!string.IsNullOrEmpty(args.Surname))
                result = result.Where(x => x.Surname.Contains(args.Email));

            if (!string.IsNullOrEmpty(args.Email))
                result = result.Where(x => x.Email == args.Email);

            return result.ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _dbSet.FirstOrDefault(x => x.Email == email);
        }
    }
}
