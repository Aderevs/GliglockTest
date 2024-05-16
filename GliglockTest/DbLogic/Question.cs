using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GliglockTest.DbLogic
{
    public class Question
    {
        public Guid Id { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public bool WithImg { get; set; }

        [Required]
        public Guid TestId { get; set; }
        public Test? Test { get; set; }
        public List<AnswerOption>? AnswerOptions { get; set; }
    }
}
