using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Contract
{
    [Serializable]
    public class TaskContract : ContractBase
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required]
        public bool IsChecked { get; set; }

        public DateTime? NotificationDate { get; set; }

        [Required]
        public int ToDoListId { get; set; }
    }
}