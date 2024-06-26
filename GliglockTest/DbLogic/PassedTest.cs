﻿using System.ComponentModel.DataAnnotations;

namespace GliglockTest.DbLogic
{
    public class PassedTest
    {
        public Guid Id { get; set; }

        [Required]
        public int Mark { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public Guid TestId { get; set; }
        public Test? Test { get; set; }
    }
}
