using System.ComponentModel.DataAnnotations;

namespace GliglockTest.DbLogic
{
    public class AnswerOption
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Content { get; set; }

        [Required]
        public bool IsCorrect { get; set; }
        
        [Required]
        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
