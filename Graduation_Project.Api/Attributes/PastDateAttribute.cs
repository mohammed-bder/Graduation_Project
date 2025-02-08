using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.Attributes
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            if (value is DateTime date)
                return date < DateTime.Now;

            return false;
        }
    }
}
