using GliglockTest.DbLogic;

namespace GliglockTest
{
    public class StudentTestTaker : BaseUser
    {
        public StudentTestTaker(TestsDbContext dbContext) : base(dbContext) {}
    }
}
