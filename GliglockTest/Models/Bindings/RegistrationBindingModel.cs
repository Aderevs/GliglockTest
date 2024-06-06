using System.ComponentModel.DataAnnotations;
using GliglockTest.appCore;

namespace GliglockTest.Models.Bindings
{
    public class RegistrationBindingModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email adders")]
        public string Email { get; set; }

        [UIHint("Date")]
        [CheckDateNotEarlierThanToday(ErrorMessage = "We do not provide services to people who have not yet been born")]
        public DateOnly? Birthday { get; set; }

        [Required]
        public bool IsTeacher { get; set; }

        [Required]
        [UIHint("Password")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z0-9\s]+$", ErrorMessage ="password is too easy")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z0-9\s!@#$%^&*()-_+=~`{}[\]:;""'<>,.?/\\|]{6,}$", ErrorMessage = "password is too easy")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm password")]
        [UIHint("Password")]
        public string PasswordConfirm { get; set; }
    }
}
