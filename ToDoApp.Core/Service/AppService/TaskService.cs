using System.Collections.Generic;
using System.Data.Entity;
using ToDoApp.Core.Persistence;
using ToDoApp.Entity.Model;
using ToDoApp.Entity.SearchArgs;

namespace ToDoApp.Core.Service.AppService
{
    public class TaskService : AppServiceBase<Task>
    {
        private readonly IUnitOfWork<DbContext> _unitOfWork;
        private readonly ITaskRepository _taskRepository;

        public TaskService(IUnitOfWork<DbContext> unitOfWork, IRepositoryFactory repositoryFactory)
        {
            _taskRepository = repositoryFactory.GeTaskRepository();
            _unitOfWork = unitOfWork;
        }

        public Task Save(Task task)
        {
            if (task.Id != default(int))
                return Update(task);

            var entity = _taskRepository.Save(task);
            _unitOfWork.Commit();
            return entity;
        }

        private Task Update(Task task)
        {
            var dbEntity = _taskRepository.Get(task.Id);
            SetUpdateFields(dbEntity, ref task);
            _taskRepository.Update(dbEntity);
            _unitOfWork.Commit();
            return dbEntity;
        }

        public IList<Task> Search(TaskSearchArgs args)
        {
            return _taskRepository.Search(args);
        }

        public Task Get(int id)
        {
            return _taskRepository.Get(id);
        }

        public bool Delete(int id)
        {
            var dbEntity = _taskRepository.Delete(id);
            _unitOfWork.Commit();
            return dbEntity;
        }

        public IList<Task> GetNotificationNotSendItems()
        {
            return _taskRepository.GetNotificationNotSendItems();
        }

        protected override void SetUpdateFields(Task dbEntity, ref Task entity)
        {
            entity = SetEntityFields(dbEntity, entity,
                Task.Properties.Title,
                Task.Properties.IsChecked,
                Task.Properties.NotificationDate,
                Task.Properties.ModifiedBy,
                Task.Properties.ModifiedOn
            );
        }
    }
}
