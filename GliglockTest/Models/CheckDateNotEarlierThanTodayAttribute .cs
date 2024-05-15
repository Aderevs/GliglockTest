using System.ComponentModel.DataAnnotations;

namespace GliglockTest.Models
{
    public class CheckDateNotEarlierThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                DateTime date;
                if (value is DateOnly dateOnly)
                {
                    date = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
                }
                else
                {
                    date = (DateTime)value;
                }
                if (date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
