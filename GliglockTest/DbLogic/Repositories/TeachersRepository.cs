using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest.DbLogic.Repositories
{
    public class TeachersRepository : ITeachersRepository
    {
        private readonly TestsDbContext _dbContext;

        public TeachersRepository(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _dbContext.Teachers.ToListAsync();
        }
        public async Task<Teacher> GetTeacherByIdAsync(Guid id)
        {
            return await _dbContext.Teachers.FirstAsync(t => t.Id == id);
        }
        public async Task<Teacher> GetTeacherByEmailAsync(string email)
        {
            return await _dbContext.Teachers.FirstAsync(t => t.Email == email);
        }
        public async Task<Teacher> GetTeacherByEmailOrDefaultAsync(string email)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == email);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<Teacher> GetTeacherByIdIncludeCreatedTestsAsync(Guid id)
        {
            return await _dbContext.Teachers
                .Include(t => t.Tests)
                .FirstAsync(t => t.Id == id);
        }
        public async Task<Teacher> GetTeacherByEmailIncludeCreatedTestsAsync(string email)
        {
            return await _dbContext.Teachers
                .Include(t => t.Tests)
                .FirstAsync(t => t.Email == email);
        }
        public Teacher GetTeacherByEmailIncludeCreatedTests(string email)
        {
            return _dbContext.Teachers
                .Include(t => t.Tests)
                .First(t => t.Email == email);
        }
        public async Task AddTeacherAsync(Teacher teacher)
        {
            _dbContext.Teachers.Add(teacher);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> CheckIfExistsTeacherWithEmailAsync(string email)
        {
            return await _dbContext.Teachers.AnyAsync(t => t.Email == email);
        }
    }
}
