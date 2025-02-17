using Graduation_Project.Core.Models;
using Graduation_Project.Core.Specifications;

namespace Graduation_Project.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public  Task AddAsync(T entity);
        public  Task<T> AddWithSaveAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task<IReadOnlyList<T>?> GetAllWithSpecAsync(ISpecifications<T> specs);
        public Task<T?> GetWithSpecsAsync(ISpecifications<T> specs);
        Task<T?> GetAsync(int id);
        Task<T?> GetWithNameAsync(string name);
        Task<IReadOnlyList<T>> GetAllAsync();
        public Task SaveAsync();
        public void DeleteRange(IEnumerable<T> entities);
        public Task AddRangeAsync(IEnumerable<T> entities);


    }
}
