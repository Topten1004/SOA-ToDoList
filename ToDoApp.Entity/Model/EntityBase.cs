using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Entity.Model
{
    public class EntityBase
    {
        [Required]
        public DateTime CreatedOn { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }
    }
}
