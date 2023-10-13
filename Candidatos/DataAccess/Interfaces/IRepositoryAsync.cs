using BD;
using System.Linq.Expressions;

namespace Candidatos.DataAccess.Interfaces
{
    public interface IRepositoryAsync<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetByID(int? id);

        Task<T> Insert(T entity);

        Task<T> Delete(int? id);

        Task Update(T entity);

        Task<T> Find(Expression<Func<T, bool>> expr);

        IEnumerable<T> Where(Func<T, bool> filter);

        IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
    }
}
