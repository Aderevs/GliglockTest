namespace GliglockTest.appCore
{
    public interface ITestBuilder
    {
        TestBuilder AddName(string name);
        TestBuilder AddQuestion(TestQuestion question);
        TestBuilder AddQuestions(List<TestQuestion> questions);
        Task<Test> BuildAsync();
        Test Build();
        void Clear();
        IQuestionBuilder CreateQuestionAndAdd(string questionContent);
        event Func<Test, Task>? OnTestBuilt;
        TestBuilder SetMaxMark(int maxMark);
        TestBuilder SetTeacherId(Guid teacherId);
    }
}