namespace GliglockTest.DbLogic.Repositories.Interfaces
{
    public interface IPassedTestsRepository
    {
        Task AddPassedTestAsync(PassedTest passedTest);
        Task<IEnumerable<PassedTest>> GetPassedTestsOfStudentByIdAsync(Guid studentId);
        Task<IEnumerable<PassedTest>> GetPassedTestsOfStudentByEmailAsync(string studentEmail);
        Task<IEnumerable<PassedTest>> GetPassedTestsOfTeachersCreatedByIdAsync(Guid teacherId);
    }
}