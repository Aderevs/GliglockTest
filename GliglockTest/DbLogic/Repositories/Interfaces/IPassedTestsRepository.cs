namespace GliglockTest.DbLogic.Repositories.Interfaces
{
    public interface IPassedTestsRepository
    {
        Task AddPassedTestAsync(PassedTest passedTest);
        Task<IEnumerable<PassedTest>> GetPassedTestsOfStudentByIdAsync(Guid studentId);
        Task<IEnumerable<PassedTest>> GetPassedTestsOfTeachersCreatedByIdAsync(Guid teacherId);
    }
}