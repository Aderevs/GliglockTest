﻿using GliglockTest.DbLogic;
using System.ComponentModel.DataAnnotations;

namespace GliglockTest
{
    public abstract class BaseUser
    {
        protected readonly TestsDbContext _dbContext;

        protected BaseUser(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Id { get; set; }
        public string? NickName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public DateOnly BirthDay { get; set; }

    }
}
