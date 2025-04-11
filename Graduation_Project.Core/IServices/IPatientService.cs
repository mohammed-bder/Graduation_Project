using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core.IServices
{
    public interface IPatientService
    {
        Task<object?> GetInfo(int Id, string? Email);
    }
}
