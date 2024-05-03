﻿using AutoMapper;
using GliglockTest.DbLogic;

namespace GliglockTest
{
    public class DbModelAdapter
    {
        private TestsDbContext _dbContext;
        private IMapper _mapper;

        public DbModelAdapter(TestsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task SaveCreatedTestToDbAsync(Test test)
        {
            var dbTest = _mapper.Map<DbLogic.Test>(test);
            _dbContext.Tests.Add(dbTest);
            var testId = test.Id; 
            foreach(var question in test.Questions)
            {
                var dbQuestion = _mapper.Map<DbLogic.TestQuestion>(question);
                dbQuestion.TestId = testId;
                _dbContext.TestsQuestions.Add(dbQuestion);
                var questionId = question.Id;
                foreach(var option in question.Options)
                {
                    var dbOption = _mapper.Map<DbLogic.AnswerOption>(option);
                    dbOption.QuestionId = questionId;
                    _dbContext.AnswersQuestions.Add(dbOption);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SavePassedTestToDbAsync(PassedTest passedTest)
        {
            var dbPassedTest = _mapper.Map<DbLogic.PassedTest>(passedTest);
            _dbContext.PassedTests.Add(dbPassedTest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
