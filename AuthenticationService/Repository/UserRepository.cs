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

    public async Task CreateAsync(UserEntity entity)
    {
        var exists = await this.context.Users.AnyAsync(x => x.Username == entity.Username);
        if (exists)
        {
            throw new InvalidOperationException($"{nameof(entity.Username)} \"{entity.Username}\" already exists");
        }
        await this.context.AddAsync(entity);
        await this.context.SaveChangesAsync();
    }

    public IAsyncEnumerable<UserEntity> GetAsync(IFilter<UserEntity> filter)
    {
        return filter.Apply(this.context.Users).AsAsyncEnumerable();
    }

    public async Task UpdateAsync(UserEntity entity)
    {
        var exists = await this.context.Users.AnyAsync(x => x.Id == entity.Id);
        if (!exists)
        {
            throw new InvalidOperationException($"Can not update {nameof(UserEntity)}: id {entity.Id} does not exist");
        }
        this.context.Update(entity);
        await this.context.SaveChangesAsync();
    }
}