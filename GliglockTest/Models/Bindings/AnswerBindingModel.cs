namespace GliglockTest.Models.Bindings
{
    public class AnswerBindingModel
    {
        public Guid QuestionId { get; set; }
        public List<Guid> SelectedOptions { get; set; }
    }
}
