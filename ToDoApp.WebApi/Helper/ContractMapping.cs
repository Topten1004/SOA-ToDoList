using System;
using ExpressMapper;
using ToDoApp.Contract;
using ToDoApp.Entity.Model;

namespace ToDoApp.WebApi.Helper
{
    public class ContractMapping
    {
        public static void MappingRegistration()
        {
            UserRegistration();
            TaskRegistration();
            ToDoListRegistration();

            Mapper.Compile();
        }

        private static void UserRegistration()
        {
            Mapper.Register<UserContract, User>()
                .Member(x => x.Name, o => o.Name.Trim())
                .Member(x => x.Surname, o => o.Surname.Trim())
                .Member(x => x.Email, o => o.Email.Trim())
                .Member(x => x.Password, o => o.Password.Trim())
                .Member(x => x.CreatedOn, o => o.CreatedOn == default(DateTime) || o.CreatedOn.Year.ToString() == "1" ? DateTime.Now : o.CreatedOn);

            Mapper.Register<User, UserContract>()
                .Ignore(x => x.Password)
                .Ignore(x => x.CreatedOn)
                .Ignore(x => x.CreatedBy)
                .Ignore(x => x.ModifiedOn)
                .Ignore(x => x.ModifiedBy);
        }

        private static void TaskRegistration()
        {
            Mapper.Register<TaskContract, Task>()
                .Member(x => x.Title, o => o.Title.Trim())
                .Member(x => x.CreatedOn, o => o.CreatedOn == default(DateTime) || o.CreatedOn.Year.ToString() == "1" ? DateTime.Now : o.CreatedOn);

            Mapper.Register<Task, TaskContract>();
        }

        private static void ToDoListRegistration()
        {
            Mapper.Register<ToDoListContract, ToDoList>()
                .Member(x => x.Title, o => o.Title.Trim())
                .Member(x => x.CreatedOn, o => o.CreatedOn == default(DateTime) || o.CreatedOn.Year.ToString() == "1" ? DateTime.Now : o.CreatedOn);

            Mapper.Register<ToDoList, ToDoListContract>();
        }
    }
}