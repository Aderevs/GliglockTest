namespace GliglockTest.DbLogic
{
    public class PassedTest
    {
        Guid Id { get; set; }
        public int Mark { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
