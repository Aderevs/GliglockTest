namespace GliglockTest.appCore.Interfaces
{
    public interface ICacheProvider
    {
        Task<Test> GetCertainTestAsync(Guid testId);
        Task<TestsListPage> GetTestsForPageAsync();
        Task<TestsListPage> GetTestsForPageAsync(ushort page);
    }
}