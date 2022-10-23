using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

namespace AuthenticationService.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<RoleEntity> roleRepository;

    public RoleService(IRepository<RoleEntity> roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    public async Task CreateRoleAsync(string role)
    {
        try
        {
            await this.roleRepository.CreateAsync(new RoleEntity() { Role = role });
        }
        catch (InvalidOperationException e)
        {
            throw new RoleExistenceException(e.Message, e);
        }
    }

    public async Task DeleteRoleAsync(string role)
    {
        var filter = new RoleFilter(role);
        var roleEntity = await this.roleRepository.GetAsync(filter).SingleOrDefaultAsync();
        roleEntity = roleEntity ?? throw new RoleExistenceException($"{nameof(RoleEntity)} \"{role}\" does not exist");
        await this.roleRepository.DeleteAsync(roleEntity);
    }
}
