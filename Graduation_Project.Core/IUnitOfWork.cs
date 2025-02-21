using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Core
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        bool HasChanges();
        Task<int> CompleteAsync();
    }
}
