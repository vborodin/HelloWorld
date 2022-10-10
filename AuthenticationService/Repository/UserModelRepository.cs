using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Entities;

namespace AuthenticationService.Repository;

public class UserModelRepository : IRepository<UserEntity>
{
    private readonly UserModelContext context;

    public UserModelRepository(UserModelContext context)
    {
        this.context = context;
    }

    public void Create(UserEntity data)
    {
        this.context.Users.Add(data);
        this.context.SaveChanges();
    }

    public IEnumerable<UserEntity> Get(IFilter<UserEntity> filter)
    {
        return filter.Apply(this.context.Users);
    }
}
