using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSST.Agile.Database.Configuration.Interfaces;

namespace TSST.Agile.Database.Configuration.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IAgileDbContext context;

        protected readonly IDbSet<T> dbset;

        public GenericRepository(IAgileDbContext dbContext)
        {
            context = dbContext;
            dbset = context.Set<T>();
        }

        public void ExecCommand(string query, params object[] parameters)
        {
            string[] splitters = new string[] { "\r\nGO\r\n", "\r\ngo\r\n" };
            string[] commands = query.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                context.Database.ExecuteSqlCommand(command, parameters);
            }
        }

        public void ExecCommand(string query)
        {
            context.Database.ExecuteSqlCommand(query);
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return context.Database.SqlQuery<T>(query, parameters);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbset.ToListAsync<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbset.ToList<T>();
        }

        public async Task<IEnumerable<T>> FindByAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = await dbset.Where(predicate).ToListAsync<T>();
            return query;
        }

        public async Task<T> EntityFindByAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await dbset.Where(predicate).FirstOrDefaultAsync<T>();
        }

        public virtual T Add(T entity)
        {
            return dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            context.SaveChanges();
        }

        public virtual void DisableChangeTracking()
        {
            context.Configuration.AutoDetectChangesEnabled = false;
        }

        public virtual void EnableChangeTracking()
        {
            context.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}
