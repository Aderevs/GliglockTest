namespace GliglockTest.appCore
{
    public class QuestionBuilder : IQuestionBuilder
    {
        private readonly Question _question;
        public QuestionBuilder()
        {
            _question = new Question();
        }

        public QuestionBuilder(Question question)
        {
            _question = question;
        }

        public QuestionBuilder AddQuestion(string question)
        {
            _question.Text = question;
            return this;
        }
        public QuestionBuilder AddAnswerOption(string answerOption, bool isCorrect)
        {
            if (_question.AnswerOptions == null)
            {
                _question.AnswerOptions = new();
            }
            _question.AnswerOptions.Add(
                new AnswerOption
                {
                    Content = answerOption,
                    IsCorrect = isCorrect
                });
            return this;
        }
        public QuestionBuilder AddAnswerOptions(List<AnswerOption> answerOptions)
        {
            if (_question.AnswerOptions == null)
            {
                _question.AnswerOptions = new();
            }
            _question.AnswerOptions.AddRange(answerOptions);
            return this;
        }

        public Question Build()
        {
            if (_question.AnswerOptions == null || !_question.AnswerOptions.Any())
            {
                throw new InvalidOperationException("Attempt to create test question without correct answer");
            }
            return _question;
        }
    }
}
