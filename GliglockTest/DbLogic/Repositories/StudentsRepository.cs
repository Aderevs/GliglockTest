using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest.DbLogic.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly TestsDbContext _dbContext;

        public StudentsRepository(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }
        public async Task<Student> GetStudentByIdAsync(Guid id)
        {
            return await _dbContext.Students.FirstAsync(s => s.Id == id);
        }
        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _dbContext.Students.FirstAsync(s => s.Email == email);
        }
        public async Task<Student> GetStudentByEmailOrDefaultAsync(string email)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Students.FirstOrDefaultAsync(t => t.Email == email);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<Student> GetStudentByIdIncludePassedTestsAsync(Guid id)
        {
            return await _dbContext.Students
                .Include(s => s.PassedTests)
                .ThenInclude(pt => pt.Test)
                .FirstAsync(s => s.Id == id);
        }
        public async Task<Student> GetStudentByEmailIncludePassedTestsAsync(string email)
        {
            return await _dbContext.Students
                .Include(s => s.PassedTests)
                .ThenInclude(pt => pt.Test)
                .FirstAsync(s => s.Email == email);
        }
        public Student GetStudentByEmailIncludePassedTests(string email)
        {
            return _dbContext.Students
                .Include(s => s.PassedTests)
                .ThenInclude(pt => pt.Test)
                .First(s => s.Email == email);
        }
        public async Task AddStudentAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> CheckIfExistsStudentWithEmailAsync(string email)
        {
            return await _dbContext.Students.AnyAsync(s => s.Email == email);
        }
    }
}
