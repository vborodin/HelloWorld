using AuthenticationService.Repository.Filter;

namespace AuthenticationService.Repository;

public interface IRepository<T>
{
    public IEnumerable<T> Get(IFilter<T> filter);

    public void Create(T data);
}
