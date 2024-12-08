using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.IRepositories;

namespace Graduation_Project.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            if(typeof(T) is Education)
                return (IEnumerable<T>?) await dbContext.Set<Education>().Include(e=>e.Doctor).ToListAsync();

            if (typeof(T) is Doctor)
                return (IEnumerable<T>?)await dbContext.Set<Doctor>().Include(d => d.Educations).Include(d => d.Specialty).ToListAsync();

            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            if (typeof(T) is Education)
                return await dbContext.Set<Education>().Where(e => e.Id == id).Include(e => e.Doctor).FirstOrDefaultAsync() as T;

            if (typeof(T) is Doctor)
                return await dbContext.Set<Doctor>().Where(e => e.Id == id).Include(e => e.Specialty).Include(e => e.Educations).FirstOrDefaultAsync() as T;

            return await dbContext.FindAsync<T>(id);
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }
    }
}
