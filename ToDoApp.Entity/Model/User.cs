using System.ComponentModel.DataAnnotations;
using ToDoApp.Common.Attributes;

namespace ToDoApp.Entity.Model
{
    public class User : EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [StringLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Encrypt]
        public string Password { get; set; }

        public struct Properties
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string Surname = "Surname";
            public const string Email = "Email";
            public const string Password = "Password";
            public const string CreatedOn = "CreatedOn";
            public const string CreatedBy = "CreatedBy";
            public const string ModifiedOn = "ModifiedOn";
            public const string ModifiedBy = "ModifiedBy";
        }
    }
}
