using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ToDoApp.Core.Persistence;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Infrastructure.Persistence
{
    public class ToDoListRepository : Repository<ToDoList, int>, IToDoListRepository
    {
        private readonly DbSet<ToDoList> _dbSet;
        public ToDoListRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException();

            _dbSet = unitOfWork.DatabaseContext.Set<ToDoList>();
        }

        public IList<ToDoList> Search(ToDoListSearchArgs args)
        {
            if (args == null)
                return null;

            var result = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(args.Title))
                result = result.Where(x => x.Title == args.Title);

            if (!args.IsChecked.HasValue)
                result = result.Where(x => x.IsChecked == args.IsChecked.Value);

            return result.ToList();
        }

        public IList<ToDoList> GetNotificationNotSendItems()
        {
            return _dbSet
                .Where(x => !x.IsNotificationSend && x.NotificationDate < DateTime.Now)
                .Include(x=> x.User)
                .ToList();
        }

        public IList<ToDoList> GetAll(int userId)
        {
            return _dbSet.Where(x => x.UserId == userId).ToList();
        }
    }
}
