using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

namespace AuthenticationService.Repository;

public interface IRepository<T>
{
    public IEnumerable<T> Get(IFilter<T> filter);
}
