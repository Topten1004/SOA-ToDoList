using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Contract
{
    [Serializable]
    public class ToDoListContract : ContractBase
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public bool IsChecked { get; set; }

        public DateTime? NotificationDate { get; set; }

        public int UserId { get; set; }
        public virtual List<TaskContract> Tasks { get; set; }
    }
}
