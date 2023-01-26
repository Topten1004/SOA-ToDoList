using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entity.Model
{
    public class ToDoList : EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        public bool IsChecked { get; set; }

        public DateTime? NotificationDate { get; set; }

        public bool IsNotificationSend { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<Task> Tasks { get; set; }

        public struct Properties
        {
            public const string Id = "Id";
            public const string Title = "Title";
            public const string IsChecked = "IsChecked";
            public const string NotificationDate = "NotificationDate";
            public const string User = "User";
            public const string Tasks = "Tasks";
            public const string CreatedOn = "CreatedOn";
            public const string CreatedBy = "CreatedBy";
            public const string ModifiedOn = "ModifiedOn";
            public const string ModifiedBy = "ModifiedBy";
        }
    }
}
