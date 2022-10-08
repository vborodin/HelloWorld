using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

namespace AuthenticationService.Repository;

public class UserModelRepository : IRepository<UserModel>
{
    private readonly DataContext context;

    public UserModelRepository(DataContext context)
    {
        this.context = context;
    }

    public IEnumerable<UserModel> Get(IFilter<UserModel> filter)
    {
        return filter.Apply(this.context.Users);
    }
}
