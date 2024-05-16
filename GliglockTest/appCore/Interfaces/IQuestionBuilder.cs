namespace GliglockTest.appCore
{
    public interface IQuestionBuilder
    {
        QuestionBuilder AddAnswerOption(string answerOption, bool isCorrect);
        QuestionBuilder AddAnswerOptions(List<AnswerOption> answerOptions);
        QuestionBuilder AddQuestion(string question);
        Question Build();
    }
}