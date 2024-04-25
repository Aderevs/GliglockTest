using System.ComponentModel.DataAnnotations.Schema;

namespace GliglockTest.DbLogic
{
    public class TestQuestion
    {
        public Guid Id { get; set; }
        public string? QuestionText { get; set; }
        public Guid TestId { get; set; }
        public Test? Test { get; set; }
        public List<AnswerOption>? AnswerOptions { get; set; }

        [NotMapped]
        public bool HasManyAnswers => AnswerOptions?.Count(ao => ao.IsCorrect) > 1;
    }
}
