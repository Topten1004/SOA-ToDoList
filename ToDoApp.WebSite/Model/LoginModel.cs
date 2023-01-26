using System.ComponentModel.DataAnnotations;

namespace ToDoApp.WebSite.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-Mail required.")]
        [Display(Name = "E-Mail")]
        [StringLength(250)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(12)]
        public string Password { get; set; }
    }
}