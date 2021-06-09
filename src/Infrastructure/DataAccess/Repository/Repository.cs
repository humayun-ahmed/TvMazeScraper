namespace Infrastructure.Repository
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	using Contracts;

	using Microsoft.EntityFrameworkCore;

	public class Repository : IRepository
    {
        private readonly BaseContext _dbContext;
        public Repository(BaseContext context) => this._dbContext = context;
        public Task<T> GetItem<T>(Expression<Func<T, bool>> filter) where T : class => this._dbContext.Set<T>().FirstOrDefaultAsync(filter);

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> filter = null) where T : class
        {
            return filter == null ? this._dbContext.Set<T>().AsNoTracking() : this._dbContext.Set<T>().AsNoTracking().Where(filter);
        }

        public T Add<T>(T model) where T : class
        {
            var entity = this._dbContext.Set<T>().Add(model).Entity;
            return entity;
        }

        public void Remove<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var entitySet = this._dbContext.Set<T>();
            entitySet.RemoveRange(entitySet.Where(filter));
        }

        public void Update<T>(T model) where T : class
        {
            this._dbContext.Set<T>().Update(model);
        }

        public Task SaveChanges() => this._dbContext.SaveChangesAsync();

        public async Task<int> CountAsync<T>() where T : class
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public virtual async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }

    }
}
