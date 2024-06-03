namespace GliglockTest.DbLogic.Repositories.Interfaces
{
    public interface ITestsRepository
    {
        Task AddTestAsync(Test test);
        Task DeleteTestAsync(Guid id);
        Task<IEnumerable<Test>> GetAllSolidTestsAsync();
        Task<IEnumerable<Test>> GetAllSolidTestsIncludeTeacherAsync();
        Task<IEnumerable<Test>> GetPaginatedSolidTestsAsync(int limit, int pageNumber);
        Task<IEnumerable<Test>> GetPaginatedSolidTestsIncludeTeacherAsync(int limit, int pageNumber);
        Task<Test> GetSolidTestByIdAsync(Guid id);
    }
}