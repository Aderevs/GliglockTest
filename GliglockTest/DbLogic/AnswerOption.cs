using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace GliglockTest.DbLogic
{
    public class AnswerOption
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public TestQuestion Question { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
