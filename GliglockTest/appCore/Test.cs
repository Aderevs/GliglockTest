namespace GliglockTest.appCore
{
    public class Test
    {
        public Test()
        {
            Questions = [];
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int MaxMark { get; set; }
        public Guid TeacherId { get; set; }
        public List<TestQuestion> Questions { get; set; }
        
    }
}
