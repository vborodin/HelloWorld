using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class RoleRepository : IRepository<RoleEntity>
{
    private readonly AppDbContext context;

    public RoleRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(RoleEntity entity)
    {
        var exists = await this.context.Roles.AnyAsync(x => x.Role == entity.Role);
        if (exists)
        {
            throw new InvalidOperationException($"Can not create {nameof(RoleEntity)}: {nameof(RoleEntity.Role)} \"{entity.Role}\" already exists");
        }

        await this.context.AddAsync(entity);
        await this.context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RoleEntity entity)
    {
        var exists = await this.context.Roles.AnyAsync(x => x.Id == entity.Id);
        if (!exists)
        {
            throw new InvalidOperationException($"Can not delete {nameof(RoleEntity)}: {nameof(RoleEntity.Id)} \"{entity.Id}\" does not exist");
        }

        this.context.Remove(entity);
        await this.context.SaveChangesAsync();
    }

    public IAsyncEnumerable<RoleEntity> GetAsync(IFilter<RoleEntity> filter)
    {
        return filter.Apply(this.context.Roles).AsAsyncEnumerable();
    }

    public async Task UpdateAsync(RoleEntity entity)
    {
        var exists = await this.context.Roles.AnyAsync(x => x.Id == entity.Id);
        if (!exists)
        {
            throw new InvalidOperationException($"Can not update {nameof(RoleEntity)}: {nameof(RoleEntity.Id)} \"{entity.Id}\" does not exist");
        }

        this.context.Update(entity);
        await this.context.SaveChangesAsync();
    }
}
