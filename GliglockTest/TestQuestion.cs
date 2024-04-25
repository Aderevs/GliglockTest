namespace GliglockTest
{
    public class TestQuestion
    {
        //public Guid TestId { get; set; }
        public Guid Id { get; set; }
        public string? Question { get; set; }
        public List<AnswerOption>? Options { get; set; }
        public bool HasManyAnswers
        {
            get
            {
                return Options?.Count(o => o.IsCorrect) > 1;
            }
        }
    }
}
