using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TSST.Agile.Database.Configuration.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> EntityFindByAsync(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void Save();
        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);
        void ExecCommand(string query, params object[] parameters);
        void ExecCommand(string query);

        void DisableChangeTracking();
        void EnableChangeTracking();
    }
}
