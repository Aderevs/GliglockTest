using GliglockTest.appCore;
using GliglockTest.DbLogic;
using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest.DbLogic.Repositories
{
    public class TestsRepository : ITestsRepository
    {
        private readonly TestsDbContext _dbContext;

        public TestsRepository(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Test>> GetAllSolidTestsAsync()
        {
            return await _dbContext.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
        }
        public async Task<IEnumerable<Test>> GetAllSolidTestsIncludeTeacherAsync()
        {
            return await _dbContext.Tests
                .Include(t => t.Teacher)
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
        }
        public async Task<IEnumerable<Test>> GetPaginatedSolidTestsAsync(int limit, int pageNumber)
        {
            return await _dbContext.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .OrderBy(t => t.Name)
                .Skip((pageNumber - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
        public async Task<IEnumerable<Test>> GetPaginatedSolidTestsIncludeTeacherAsync(int limit, int pageNumber)
        {
            return await _dbContext.Tests
                .Include(t => t.Teacher)
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .OrderBy(t => t.Name)
                .Skip((pageNumber - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
        public async Task<Test> GetSolidTestByIdAsync(Guid id)
        {
            return await _dbContext.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstAsync(t => t.Id == id);
        }
        public async Task AddTestAsync(Test test)
        {
            if (test == null)
            {
                throw new ArgumentNullException(nameof(test));
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Tests.Add(test);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteTestAsync(Guid id)
        {
            var test = await _dbContext.Tests.FindAsync(id);
            if (test != null)
            {
                _dbContext.Tests.Remove(test);
            }
        }
    }
}
