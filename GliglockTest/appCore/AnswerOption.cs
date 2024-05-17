using System.ComponentModel.DataAnnotations;

namespace GliglockTest.appCore
{
    public class AnswerOption
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
