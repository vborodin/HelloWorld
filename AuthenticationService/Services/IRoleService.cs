namespace AuthenticationService.Services;

public interface IRoleService
{
    Task CreateRoleAsync(string role);

    Task DeleteRoleAsync(string role);
}
