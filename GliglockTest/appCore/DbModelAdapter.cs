using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.DbLogic.Repositories.Interfaces;

namespace GliglockTest.appCore
{
    public class DbModelAdapter
    {
        private readonly TestsDbContext _dbContext;
        private readonly ITestsRepository _testsRepository;
        private readonly IPassedTestsRepository _passedTestsRepository;
        private readonly IMapper _mapper;

        public DbModelAdapter(
            TestsDbContext dbContext, 
            IMapper mapper, 
            ITestsRepository testsRepository, 
            IPassedTestsRepository passedTestsRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _testsRepository = testsRepository;
            _passedTestsRepository = passedTestsRepository;
        }
        public DbModelAdapter(
            TestsDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task SaveCreatedTestToDbAsync(Test test)
        {
            var dbTest = _mapper.Map<DbLogic.Test>(test);
            await _testsRepository.AddTestAsync(dbTest);
            /*_dbContext.Tests.Add(dbTest);
            var testId = test.Id;
            foreach (var question in test.Questions)
            {
                var dbQuestion = _mapper.Map<DbLogic.Question>(question);
                dbQuestion.TestId = testId;
                _dbContext.Questions.Add(dbQuestion);
                var questionId = question.Id;
                foreach (var option in question.AnswerOptions)
                {
                    var dbOption = _mapper.Map<DbLogic.AnswerOption>(option);
                    dbOption.QuestionId = questionId;
                    _dbContext.AnswerOptions.Add(dbOption);
                }
            }
            await _dbContext.SaveChangesAsync();*/
        }

        public async Task SavePassedTestToDbAsync(PassedTest passedTest)
        {
            var dbPassedTest = _mapper.Map<DbLogic.PassedTest>(passedTest);
            await _passedTestsRepository.AddPassedTestAsync(dbPassedTest);
            /*_dbContext.PassedTests.Add(dbPassedTest);
            await _dbContext.SaveChangesAsync();*/
        }
    }
}
