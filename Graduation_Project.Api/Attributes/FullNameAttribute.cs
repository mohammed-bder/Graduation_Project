using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.Attributes
{
    public class FullNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string fullName)
            {
                var nameParts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (nameParts.Length >= 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
