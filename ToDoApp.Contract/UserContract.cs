using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Contract
{
    [Serializable]
    public class UserContract : ContractBase
    {
        [Editable(false)]
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
        [StringLength(12)]
        public string Password { get; set; }
    }
}
