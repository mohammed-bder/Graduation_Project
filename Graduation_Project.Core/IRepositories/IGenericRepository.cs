namespace Graduation_Project.Core.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task<IEnumerable<T>?> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task SaveAsync();

    }
}
