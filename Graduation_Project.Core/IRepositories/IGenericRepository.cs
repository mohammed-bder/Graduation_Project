using Graduation_Project.Core.Models;
using Graduation_Project.Core.Specifications;
using System.Linq.Expressions;

namespace Graduation_Project.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task<IReadOnlyList<T>?> GetAllWithSpecAsync(ISpecifications<T> specs);
        public Task<T?> GetWithSpecsAsync(ISpecifications<T> specs);
        public Task<IReadOnlyList<TResult>> GetAllWithSpecAsync<TResult>(ISpecifications<T> spec, Expression<Func<T, TResult>> selector);
        Task<T?> GetAsync(int id);
        Task<T?> GetWithNameAsync(string name);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> GetCountAsync(ISpecifications<T> spec);
        public Task SaveAsync();
        public void DeleteRange(IEnumerable<T> entities);
        public Task AddRangeAsync(IEnumerable<T> entities);


    }
}
