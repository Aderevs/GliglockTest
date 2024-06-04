namespace GliglockTest.DbLogic.Repositories.Interfaces
{
    public interface ITeachersRepository
    {
        Task AddTeacherAsync(Teacher teacher);
        Task<bool> CheckIfExistsTeacherWithEmailAsync(string email);
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher> GetTeacherByEmailAsync(string email);
        Task<Teacher> GetTeacherByEmailIncludeCreatedTestsAsync(string email);
        Teacher GetTeacherByEmailIncludeCreatedTests(string email);
        Task<Teacher> GetTeacherByEmailOrDefaultAsync(string email);
        Task<Teacher> GetTeacherByIdAsync(Guid id);
        Task<Teacher> GetTeacherByIdIncludeCreatedTestsAsync(Guid id);
    }
}