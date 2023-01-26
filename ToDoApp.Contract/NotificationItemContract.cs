using System;

namespace ToDoApp.Contract
{
    [Serializable]
    public class NotificationItemContract
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
