using System.Runtime.InteropServices;

namespace GliglockTest
{
    public class QuestionBuilder : IQuestionBuilder
    {
        private readonly TestQuestion _question;
        public QuestionBuilder()
        {
            _question = new TestQuestion();
        }

        public QuestionBuilder(TestQuestion question)
        {
            _question = question;
        }

        public QuestionBuilder AddQuestion(string question)
        {
            _question.Question = question;
            return this;
        }
        public QuestionBuilder AddAnswerOption(string answerOption, bool isCorrect)
        {
            if (_question.Options == null)
            {
                _question.Options = new();
            }
            _question.Options.Add(
                new AnswerOption
                {
                    Content = answerOption,
                    IsCorrect = isCorrect
                });
            return this;
        }
        public QuestionBuilder AddAnswerOptions(List<AnswerOption> answerOptions)
        {
            if (_question.Options == null)
            {
                _question.Options = new();
            }
            _question.Options.AddRange(answerOptions);
            return this;
        }

        public TestQuestion Build()
        {
            if (_question.Options == null || !_question.Options.Any())
            {
                throw new InvalidOperationException("Attempt to create test question without correct answer");
            }
            return _question;
        }
    }
}
