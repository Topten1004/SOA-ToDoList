using System.Data.Entity;
using ToDoApp.Core.Persistence;
using ToDoApp.Entity.Model;

namespace ToDoApp.Core.Service.AppService
{
    public class UserService : AppServiceBase<User>
    {
        private readonly IUnitOfWork<DbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork<DbContext> unitOfWork, IRepositoryFactory repositoryFactory)
        {
            _userRepository = repositoryFactory.GetUserRepository();
            _unitOfWork = unitOfWork;
        }

        public User Save(User user)
        {
            if (user.Id != default(int))
                return Update(user);

            var savedUser = _userRepository.Save(user);
            _unitOfWork.Commit();
            return savedUser;
        }

        private User Update(User user)
        {
            var dbEntity = _userRepository.Get(user.Id);
            SetUpdateFields(dbEntity, ref user);
            _userRepository.Update(dbEntity);
            _unitOfWork.Commit();
            return dbEntity;
        }

        public User Get(int id)
        {
            return _userRepository.Get(id);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        protected override void SetUpdateFields(User dbEntity, ref User entity)
        {
            entity = SetEntityFields(dbEntity, entity,
                User.Properties.Name,
                User.Properties.Surname,
                User.Properties.Email,
                User.Properties.Password,
                User.Properties.ModifiedBy,
                User.Properties.ModifiedOn
            );
        }
    }
}
