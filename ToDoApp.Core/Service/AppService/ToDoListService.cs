using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ToDoApp.Core.Persistence;
using ToDoApp.Entity.Model;

namespace ToDoApp.Core.Service.AppService
{
    public class ToDoListService : AppServiceBase<ToDoList>
    {
        private readonly IUnitOfWork<DbContext> _unitOfWork;
        private readonly IToDoListRepository _toDoListRepository;

        public ToDoListService(IUnitOfWork<DbContext> unitOfWork, IRepositoryFactory repositoryFactory)
        {
            _toDoListRepository = repositoryFactory.GeToDoListRepository();
            _unitOfWork = unitOfWork;
        }

        public List<ToDoList> GetAll(int userId)
        {
            return _toDoListRepository.GetAll(userId).ToList();
        }

        public ToDoList Save(ToDoList toDoList)
        {
            if (toDoList.Id != default(int))
                return Update(toDoList);

            var entity = _toDoListRepository.Save(toDoList);
            _unitOfWork.Commit();
            return entity;
        }

        private ToDoList Update(ToDoList toDoList)
        {
            var dbEntity = _toDoListRepository.Get(toDoList.Id);
            SetUpdateFields(dbEntity, ref toDoList);
            _toDoListRepository.Update(dbEntity);
            _unitOfWork.Commit();
            return dbEntity;
        }

        public bool Delete(int id)
        {
            var dbEntity = _toDoListRepository.Delete(id);
            _unitOfWork.Commit();
            return dbEntity;
        }

        public ToDoList Get(int id)
        {
            return _toDoListRepository.Get(id);
        }

        public IList<ToDoList> GetNotificationNotSendItems()
        {
            return _toDoListRepository.GetNotificationNotSendItems();
        }

        protected override void SetUpdateFields(ToDoList dbEntity, ref ToDoList entity)
        {
            entity = SetEntityFields(dbEntity, entity,
                ToDoList.Properties.Title,
                ToDoList.Properties.IsChecked,
                ToDoList.Properties.NotificationDate,
                ToDoList.Properties.ModifiedBy,
                ToDoList.Properties.ModifiedOn
            );
        }
    }
}
