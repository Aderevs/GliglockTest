using GliglockTest.DbLogic;
using System.ComponentModel.DataAnnotations;

namespace GliglockTest.appCore.Account
{
    public abstract class BaseUser
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public DateOnly? BirthDay { get; set; }

    }
}
