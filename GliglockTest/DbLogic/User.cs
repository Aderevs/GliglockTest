using System.ComponentModel.DataAnnotations;

namespace GliglockTest.DbLogic
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public Guid Salt { get; set; }

        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public DateOnly BirthDay { get; set; }
    }
}
