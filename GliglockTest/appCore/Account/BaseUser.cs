using GliglockTest.DbLogic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GliglockTest.appCore.Account
{
    public abstract class BaseUser: IdentityUser
    {
        protected readonly TestsDbContext _dbContext;

        protected BaseUser(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public DateOnly? BirthDay { get; set; }

    }
}
