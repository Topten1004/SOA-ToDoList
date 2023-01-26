using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using ToDoApp.Contract;
using ToDoApp.Core.Persistence;
using ToDoApp.Core.Service;
using ToDoApp.Core.Service.AppService;
using ToDoApp.Entity.Model;
using ToDoApp.WebApi.Utility;

namespace ToDoApp.WebApi.Tasks
{
    public class NotificationSender
    {
        private readonly ToDoListService _toDoListService;
        private readonly TaskService _taskService;
        

        public NotificationSender()
        {
            var unitOfWork = IoCUtility.Resolve<IUnitOfWork<DbContext>>("isolatedUnitOfWork");
            var repositoryFactory = IoCUtility.Resolve<IRepositoryFactory>("isolatedRepositoryFactory");
            
            _toDoListService = new ToDoListService(unitOfWork, repositoryFactory);
            _taskService = new TaskService(unitOfWork, repositoryFactory);
        }

        public void Execute()
        {
            IList<ToDoList> toDoLists;
            IList<Task> tasks;
            List<NotificationItemContract> notificationItems = GetAvailableItemsFromDb(out toDoLists, out tasks);
            SendNotifications(notificationItems);
            UpdateDatabase(toDoLists, tasks);
        }

        private void UpdateDatabase(IList<ToDoList> toDoLists, IList<Task> tasks)
        {
            if (toDoLists.Any())
            {
                foreach (var toDoList in toDoLists)
                {
                    toDoList.IsNotificationSend = true;
                    _toDoListService.Save(toDoList);
                }
            }

            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    task.IsNotificationSend = true;
                    _taskService.Save(task);
                }
            }
        }

        private void SendNotifications(List<NotificationItemContract> notificationItems)
        {
            var mailNotificationService = IoCUtility.Resolve<INotificationService>();
            foreach (var notificationItem in notificationItems)
            {
                string bodyMessage = string.Format("Hi {0}, <br/>This notification for: {1}", notificationItem.FullName, notificationItem.Title);
                mailNotificationService.SendNotification(
                    ConfigurationManager.AppSettings["NotificationFromMail"],
                    notificationItem.Email,
                    ConfigurationManager.AppSettings["NotificationMailSubject"],
                    bodyMessage
                );
            }
        }

        private List<NotificationItemContract> GetAvailableItemsFromDb(out IList<ToDoList> toDoLists, out IList<Task> tasks)
        {
            List<NotificationItemContract> notificationItems = new List<NotificationItemContract>();

            toDoLists = _toDoListService.GetNotificationNotSendItems();
            foreach (var item in toDoLists)
            {
                notificationItems.Add(new NotificationItemContract
                {
                    Email = item.User.Email,
                    Title = item.Title,
                    FullName = item.User.Name + " " + item.User.Surname
                });
            }

            tasks = _taskService.GetNotificationNotSendItems();
            foreach (var item in tasks)
            {
                notificationItems.Add(new NotificationItemContract
                {
                    Email = item.ToDoList.User.Email,
                    Title = item.Title,
                    FullName = item.ToDoList.User.Name + " " + item.ToDoList.User.Surname
                });
            }

            return notificationItems;
        }
    }
}