using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Api.Attributes
{
    public class ValidEnumValueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null)
                return ValidationResult.Success;

            if(Enum.IsDefined(typeof(BloodType),value))
                return ValidationResult.Success;

            return new ValidationResult($"Invalid value for {validationContext.DisplayName}.");
        }
    }
}
