using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entity.Model
{
    public class Task : EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required]
        public bool IsChecked { get; set; }

        public DateTime? NotificationDate { get; set; }

        public bool IsNotificationSend { get; set; }

        public int ToDoListId { get; set; }
        public virtual ToDoList ToDoList { get; set; }

        public struct Properties
        {
            public const string Id = "Id";
            public const string Title = "Title";
            public const string IsChecked = "IsChecked";
            public const string NotificationDate = "NotificationDate";
            public const string ToDoList = "ToDoList";
            public const string CreatedOn = "CreatedOn";
            public const string CreatedBy = "CreatedBy";
            public const string ModifiedOn = "ModifiedOn";
            public const string ModifiedBy = "ModifiedBy";
        }
    }
}