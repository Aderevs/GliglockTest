using System.Numerics;

namespace GliglockTest.appCore
{
    public class TestBuilder : ITestBuilder
    {
        private Test _test;
        public event Func<Test, Task>? OnTestBuilt;
        public TestBuilder()
        {
            _test = new Test();
        }

        public TestBuilder AddName(string name)
        {
            _test.Name = name;
            return this;
        }
        public TestBuilder SetMaxMark(int maxMark)
        {
            _test.MaxMark = maxMark;
            return this;
        }
        public TestBuilder SetTeacherId(Guid teacherId)
        {
            _test.TeacherId = teacherId;
            return this;
        }
        public TestBuilder AddQuestion(TestQuestion question)
        {
            _test.Questions.Add(question);
            return this;
        }
        public TestBuilder AddQuestions(List<TestQuestion> questions)
        {
            _test.Questions.AddRange(questions);
            return this;
        }

        public IQuestionBuilder CreateQuestionAndAdd(string questionContent)
        {
            var question = new TestQuestion()
            /*{
                TestId = _test.Id
            }*/;
            var qBuilder = new QuestionBuilder(question);
            qBuilder.AddQuestion(questionContent);
            _test.Questions.Add(question);
            return qBuilder;
        }

        public async Task<Test> BuildAsync()
        {
            if (OnTestBuilt != null)
            {
                await OnTestBuilt(_test);
            }
            return _test;
        }
        public Test Build()
        {
            return _test;
        }
        public void Clear()
        {
            _test = new Test();
        }
    }
}
