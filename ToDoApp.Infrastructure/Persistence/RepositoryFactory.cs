using ToDoApp.Core.Persistence;

namespace ToDoApp.Infrastructure.Persistence
{
    public sealed class RepositoryFactory : IRepositoryFactory
    {
        private readonly UnitOfWork _unitOfWork;

        public RepositoryFactory(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IUserRepository GetUserRepository()
        {
            return new UserRepository(_unitOfWork);
        }

        public IToDoListRepository GeToDoListRepository()
        {
            return new ToDoListRepository(_unitOfWork);
        }

        public ITaskRepository GeTaskRepository()
        {
            return new TaskRepository(_unitOfWork);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}
