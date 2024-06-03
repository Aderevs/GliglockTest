using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest.DbLogic.Repositories
{
    public class PassedTestsRepository : IPassedTestsRepository
    {
        private readonly TestsDbContext _dbContext;

        public PassedTestsRepository(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<PassedTest>> GetPassedTestsOfStudentByIdAsync(Guid studentId)
        {
            return await _dbContext.PassedTests
                .Where(pt => pt.StudentId == studentId)
                .Include(pt=>pt.Student)
                .Include(pt => pt.Test)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
        }
        public async Task<IEnumerable<PassedTest>> GetPassedTestsOfStudentByEmailAsync(string studentEmail)
        {
            return await _dbContext.PassedTests
                .Include(pt => pt.Student)
                .Where(pt => pt.Student.Email == studentEmail)
                .Include(pt => pt.Test)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
        }
        public async Task<IEnumerable<PassedTest>> GetPassedTestsOfTeachersCreatedByIdAsync(Guid teacherId)
        {
            return await _dbContext.PassedTests
                .Include(pt => pt.Student)
                .Include(pt => pt.Test)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .Where(pt => pt.Test.TeacherId == teacherId)
                .ToListAsync();
        }
        public async Task AddPassedTestAsync(PassedTest passedTest)
        {
            _dbContext.PassedTests.Add(passedTest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
