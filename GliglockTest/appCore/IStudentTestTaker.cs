
namespace GliglockTest.appCore
{
    public interface IStudentTestTaker
    {
        List<PassedTest> PassedTests { get; set; }

        Task SaveTestResultAsync(PassedTest completedTest);
    }
}