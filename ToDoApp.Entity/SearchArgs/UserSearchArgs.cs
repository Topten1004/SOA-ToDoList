using System;

namespace ToDoApp.Entity.SearchArgs
{
    [Serializable]
    public class UserSearchArgs
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
