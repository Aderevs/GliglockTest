using System.ComponentModel.DataAnnotations;

namespace GliglockTest.Models
{
    public class AuthenticationBindingModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email adders")]
        public string Email { get; set; }

        [Required]
        public bool IsTeacher { get; set; }

        [Required]
        [UIHint("Password")]
        public string Password { get; set; }
    }
}
