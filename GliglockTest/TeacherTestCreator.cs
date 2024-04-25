using GliglockTest.DbLogic;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace GliglockTest
{
    public class TeacherTestCreator : BaseUser
    {
        private readonly ITestBuilder _testBuilder;
        public List<Test>? CreatedTests { get; set; }
        public TeacherTestCreator(TestsDbContext dbContext) : base(dbContext)
        {
            _testBuilder = new TestBuilder();
            _testBuilder.OnTestBuilt += CreateTest;
        }
        public ITestBuilder CreateTestWithBuilder(string name)
        {
            _testBuilder.Clear();
            _testBuilder.AddName(name)
                .SetTeacherId(Id);
            return _testBuilder;
        }
        public async Task CreateTest(Test test)
        {
            test.TeacherId = Id;
            var dbModel = ModelConverter.TestToDbModel(test);
            var questions = ModelConverter.AllQuestionsFromTestToDbModels(test);
            foreach( var question in questions )
            {
                _dbContext.TestsQuestions.Add(question);
            }
            foreach( var question in test.Questions)
            {
                var options = ModelConverter.AllOptionsFromQuestionToDbModel(question);
                foreach( var option in options)
                {
                    _dbContext.AnswersQuestions.Add(option);
                }
            }
            _dbContext.Tests.Add(dbModel);
            await _dbContext.SaveChangesAsync();

        }
    }
}
