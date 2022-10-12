using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class UserRepository : IRepository<UserEntity>
{
    private readonly UserModelContext context;

    public UserRepository(UserModelContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(UserEntity data)
    {
        await this.context.Users.AddAsync(data);
        await this.context.SaveChangesAsync();
    }

    public IAsyncEnumerable<UserEntity> GetAsync(IFilter<UserEntity> filter)
    {
        return filter.Apply(this.context.Users).AsAsyncEnumerable();
    }
}