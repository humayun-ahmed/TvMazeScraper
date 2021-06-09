namespace Infrastructure.Repository.Contracts
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public interface IReadOnlyRepository
    {
        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> filter = null) where T : class;
        Task<T> GetItem<T>(Expression<Func<T, bool>> filter) where T : class;
        Task<int> CountAsync<T>() where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
	}
}
