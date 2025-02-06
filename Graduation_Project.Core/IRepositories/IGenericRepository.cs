using Graduation_Project.Core.Models;
using Graduation_Project.Core.Specifications;

namespace Graduation_Project.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task<IEnumerable<T>?> GetAllWithSpecAsync(ISpecifications<T> specs);
        public Task<T?> GetWithSpecsAsync(ISpecifications<T> specs);
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        public Task SaveAsync();

    }
}
