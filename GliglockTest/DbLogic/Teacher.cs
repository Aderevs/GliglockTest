namespace GliglockTest.DbLogic
{
    public class Teacher : User
    {
        public int NumberOfCreatedTests {  get; set; }
        public List<Test> Tests { get; set; }

    }
}
