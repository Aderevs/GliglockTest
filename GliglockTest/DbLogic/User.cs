using System.ComponentModel.DataAnnotations;

namespace GliglockTest.DbLogic
{
    public abstract class User
    {
        public Guid Id { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public Guid Salt { get; set; }


        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
