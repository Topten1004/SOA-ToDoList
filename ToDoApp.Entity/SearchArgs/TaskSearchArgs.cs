using System;

namespace ToDoApp.Entity.SearchArgs
{
    [Serializable]
    public class TaskSearchArgs
    {
        public string Title { get; set; }
        public int? ToDoListId { get; set; }
    }
}
