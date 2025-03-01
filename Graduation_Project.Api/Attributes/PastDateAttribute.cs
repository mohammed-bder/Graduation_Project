using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.Attributes
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is DateOnly date)
                return date < DateOnly.FromDateTime(DateTime.Now);

            return false;
        }
    }
}
