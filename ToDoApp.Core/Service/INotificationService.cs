namespace ToDoApp.Core.Service
{
    public interface INotificationService
    {
        bool SendNotification(string messageFrom, string messageTo, string messageTitle, string messageBody);
    }
}
