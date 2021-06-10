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
        private readonly BaseContext m_dbContext;
        public Repository(BaseContext context) => this.m_dbContext = context;
        public Task<T> GetItem<T>(Expression<Func<T, bool>> filter) where T : class => this.m_dbContext.Set<T>().FirstOrDefaultAsync(filter);

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> filter = null) where T : class
        {
            return filter == null ? this.m_dbContext.Set<T>().AsNoTracking() : this.m_dbContext.Set<T>().AsNoTracking().Where(filter);
        }

        public T Add<T>(T model) where T : class
        {
            var entity = this.m_dbContext.Set<T>().Add(model).Entity;
            return entity;
        }

        public void Remove<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var entitySet = this.m_dbContext.Set<T>();
            entitySet.RemoveRange(entitySet.Where(filter));
        }

        public void Update<T>(T model) where T : class
        {
            this.m_dbContext.Set<T>().Update(model);
        }

        public Task SaveChanges() => this.m_dbContext.SaveChangesAsync();

        public async Task<int> CountAsync<T>() where T : class
        {
            return await m_dbContext.Set<T>().CountAsync();
        }

        public virtual async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await m_dbContext.Set<T>().AnyAsync(predicate);
        }

    }
}
