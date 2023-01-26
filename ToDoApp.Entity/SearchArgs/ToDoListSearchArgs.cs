using System;

namespace ToDoApp.Entity.SearchArgs
{
    [Serializable]
    public class ToDoListSearchArgs
    {
        public string Title { get; set; }
        public bool? IsChecked { get; set; }
    }
}
