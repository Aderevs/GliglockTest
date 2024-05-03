using AutoMapper;
using GliglockTest.DbLogic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest
{
    public class StudentTestTaker : BaseUser
    {
        private readonly IMapper _mapper;
        private readonly DbModelAdapter _modelAdapter;
        public List<PassedTest> PassedTests { get; set; }
        public StudentTestTaker(TestsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            var dbPassedTests = _dbContext.PassedTests
                .Where(pt=>pt.StudentId==Id)
                .Include(pt=>pt.Test)
                .ToList();
            PassedTests = _mapper.Map<List<PassedTest>>(dbPassedTests);
            _modelAdapter = new DbModelAdapter(_dbContext, _mapper);
        }

        public async Task SaveTestResultAsync(PassedTest completedTest)
        {
            if (completedTest == null) 
            {
                throw new ArgumentNullException(nameof(completedTest)); 
            }
            completedTest.Student = this;
            PassedTests.Add(completedTest);
            await _modelAdapter.SavePassedTestToDbAsync(completedTest);

        }

    }
}
