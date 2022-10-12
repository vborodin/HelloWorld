namespace AuthenticationService.Repository.Filter;

public interface IFilter<T>
{
    public IQueryable<T> Apply(IQueryable<T> source);
}
