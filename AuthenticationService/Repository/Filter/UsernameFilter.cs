using AuthenticationService.Repository.Model;

namespace AuthenticationService.Repository.Filter;

public class UsernameFilter : IFilter<UserModel>
{
    private readonly string username;

    public UsernameFilter(string username)
    {
        this.username = username;
    }

    public IEnumerable<UserModel> Apply(IEnumerable<UserModel> source)
    {
        return source.Where(x => x.Username == this.username);
    }
}
