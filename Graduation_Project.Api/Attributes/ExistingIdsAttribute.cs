using System.ComponentModel.DataAnnotations;
using Graduation_Project.Core;
using Graduation_Project.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Api.Attributes
{
    public class ExistingIdsAttribute<T> : ValidationAttribute where T : BaseEntity
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not int[] ids || ids.Length == 0)
                return ValidationResult.Success; // No IDs, assume valid

            var unitOfWork = validationContext.GetService<IUnitOfWork>();
            if (unitOfWork == null)
                throw new InvalidOperationException("DbContext is not available.");

            var dbSet = unitOfWork.Repository<T>().GetAllAsync().Result;

            var existingIds = dbSet.Select(e => e.Id).ToHashSet();
            var invalidIds = ids.Except(existingIds).ToList();

            return invalidIds.Any()
                ? new ValidationResult($"Invalid Sub_Specilaities_IDs: {string.Join(", ", invalidIds)}")
                : ValidationResult.Success;
        }
    }
}
