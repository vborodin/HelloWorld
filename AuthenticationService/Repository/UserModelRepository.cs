using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

namespace AuthenticationService.Repository;

public class UserModelRepository : IRepository<UserModel>
{
    private readonly UserModelContext context;

    public UserModelRepository(UserModelContext context)
    {
        this.context = context;
    }

    public void Create(UserModel data)
    {
        this.context.Users.Add(data);
        this.context.SaveChanges();
    }

    public IEnumerable<UserModel> Get(IFilter<UserModel> filter)
    {
        return filter.Apply(this.context.Users);
    }
}
