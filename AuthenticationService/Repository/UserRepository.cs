using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class UserRepository : IRepository<UserEntity>
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(UserEntity data)
    {
        await this.context.AddAsync(data);
        await this.context.SaveChangesAsync();
    }

    public IAsyncEnumerable<UserEntity> GetAsync(IFilter<UserEntity> filter)
    {
        return filter.Apply(this.context.Users).AsAsyncEnumerable();
    }

    public async Task UpdateAsync(UserEntity data)
    {
        var exists = await this.context.Users.AnyAsync(x => x.Id == data.Id);
        if (!exists)
        {
            throw new InvalidOperationException($"Can not update {nameof(UserEntity)}: id {data.Id} does not exist");
        }
        this.context.Update(data);
        await this.context.SaveChangesAsync();
    }
}