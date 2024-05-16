namespace GliglockTest.Models
{
    public abstract class BaseUserView
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public DateOnly? BirthDay { get; set; }
    }
}
