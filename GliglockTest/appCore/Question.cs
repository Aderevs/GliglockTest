namespace GliglockTest.appCore
{
    public class Question
    {
        private List<AnswerOption>? _studentsAnswer;
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public List<AnswerOption>? AnswerOptions { get; set; }
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
        public bool HasManyAnswers => AnswerOptions?.Count(o => o.IsCorrect) > 1;
        public void AddStudentsAnswer(AnswerOption answer)
        {
            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer));
            }
            if (AnswerOptions != null && AnswerOptions.Contains(answer))
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
        public void AddStudentsAnswer(IEnumerable<AnswerOption> answers)
        {
            foreach(var answer in answers)
            {
                AddStudentsAnswer(answer);
            }
        }

        public void ClearStudentAnswer()
        {
            _studentsAnswer?.Clear();
        }
    }
}
