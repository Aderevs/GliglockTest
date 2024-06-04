namespace GliglockTest.DbLogic.Repositories.Interfaces
{
    public interface IStudentsRepository
    {
        Task AddStudentAsync(Student student);
        Task<bool> CheckIfExistsStudentWithEmailAsync(string email);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByEmailAsync(string email);
        Task<Student> GetStudentByEmailIncludePassedTestsAsync(string email);
        Task<Student> GetStudentByEmailOrDefaultAsync(string email);
        Task<Student> GetStudentByIdAsync(Guid id);
        Task<Student> GetStudentByIdIncludePassedTestsAsync(Guid id);
    }
}