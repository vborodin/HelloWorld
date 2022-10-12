using AuthenticationService.Repository.Entities;

namespace AuthenticationService.Repository.Filter;

public class UsernameFilter : IFilter<UserEntity>
{
    private readonly string username;

    public UsernameFilter(string username)
    {
        this.username = username;
    }

    public IQueryable<UserEntity> Apply(IQueryable<UserEntity> source)
    {
        return source.Where(x => x.Username == this.username);
    }
}
