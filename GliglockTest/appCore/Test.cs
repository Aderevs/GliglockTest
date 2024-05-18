using GliglockTest.Models;

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
        public ushort MaxMark { get; set; }
        public Guid TeacherId { get; set; }
        public TeacherView Teacher { get; set; }
        public List<Question> Questions { get; set; }

    }
}
