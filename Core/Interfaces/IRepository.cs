using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IRepository<T> where T : class
	{
 		Task<IEnumerable<T>> GetAllAsync();

 		Task<T> GetByIdAsync(Guid id);

 		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

 		Task AddAsync(T entity);

 		Task AddRangeAsync(IEnumerable<T> entities);

 		void Update(T entity);

 		void Delete(T entity);

 		void DeleteRange(IEnumerable<T> entities);

		IQueryable<T> Query();

		Task<int> SaveChangesAsync();
        Task<int> GetTotalCourseCountAsync();

    }


}
