using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models;
using Graduation_Project.Core.Specifications;
using Graduation_Project.Core.Specifications.DoctorSpecifications;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
             await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync(); // Save changes immediately to get the ID

            return entity;
        }

        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>?> GetAllWithSpecAsync(ISpecifications<T> specs)
        {
            return await ApplyQuery(specs).ToListAsync();
        }

        public async Task<T?> GetWithSpecsAsync(ISpecifications<T> specs)
        {
            return await ApplyQuery(specs).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

            return await dbContext.Set<T>().ToListAsync();
        }


        public async Task<T?> GetAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetWithNameAsync(string name)
        {
            return await dbContext.Set<T>().FindAsync(name);

        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public IQueryable<T> ApplyQuery(ISpecifications<T> specs) //Helper Method
        {
            return SpecificationsEvaluator<T>.GetQuery(dbContext.Set<T>(), specs);
        }

    
    }
}
