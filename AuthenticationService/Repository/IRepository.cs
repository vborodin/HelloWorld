using AuthenticationService.Repository.Filter;

namespace AuthenticationService.Repository;

public interface IRepository<T>
{
    public IAsyncEnumerable<T> GetAsync(IFilter<T> filter);

    public Task CreateAsync(T entity);

    public Task UpdateAsync(T entity);

    public Task DeleteAsync(T entity);
}
