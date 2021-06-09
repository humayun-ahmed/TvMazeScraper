using System.Linq;

namespace Infrastructure.Repository.Contracts.Filter
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IQueryable<T> Results { get; set; }
    }
}