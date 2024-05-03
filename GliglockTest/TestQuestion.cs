namespace GliglockTest
{
    public class TestQuestion
    {
        private List<AnswerOption>? _studentsAnswer;
        public Guid Id { get; set; }
        public string? QuestionText { get; set; }
        public List<AnswerOption>? Options { get; set; }
        public AnswerOption[] StudentsAnswer
        {
            get
            {
                if (_studentsAnswer == null)
                {
                    return Array.Empty<AnswerOption>();
                }
                return _studentsAnswer.ToArray();
            }
        }
        public bool HasStudentAnswer
        {
            get
            {
                return _studentsAnswer != null;
            }
        }
        public bool HasManyAnswers
        {
            get
            {
                return Options?.Count(o => o.IsCorrect) > 1;
            }
        }
        public void AddStudentsAnswer(AnswerOption answer)
        {
            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer));
            }
            if ((bool)Options?.Contains(answer))
            {
                if (_studentsAnswer == null)
                {
                    _studentsAnswer = new List<AnswerOption>();
                }
                _studentsAnswer.Add(answer);
            }
            else
            {
                throw new InvalidOperationException("attempt to set student's answer with option which doesn't exist in answer options");
            }
        }
    }
}
