using Graduation_Project.Core;
using Graduation_Project.Core.IRepositories;
using Graduation_Project.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbcontext;
        //private Dictionary<string, GenericRepository<BaseEntity>> repositories;
        private Hashtable repositories;
        //Hashtable(non generic collection) can't be used in one application in which boxing and unboxing takes place 
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbcontext = dbContext;
            repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(dbcontext);

                repositories.Add(key, repository);
            }

            return repositories[key] as IGenericRepository<TEntity>;
        }

        public bool HasChanges()
        {
            return dbcontext.ChangeTracker.HasChanges(); // Check for pending changes
        }
        public async Task<int> CompleteAsync()
            => await dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await dbcontext.DisposeAsync();

    }
}
