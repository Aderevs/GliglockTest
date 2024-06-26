﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GliglockTest.DbLogic
{
    public class Test
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [DefaultValue(100)]
        public int MaxMark {  get; set; }
        public Guid TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Question>? Questions { get; set; }
        public List<PassedTest>? Results { get; set; }
    }
}
