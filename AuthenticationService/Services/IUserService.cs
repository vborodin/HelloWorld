using AuthenticationService.Services.Model;

namespace AuthenticationService.Services;

public interface IUserService
{
    Task<UserModel?> GetUserAsync(string username, string password);

    Task CreateUserAsync(string username, string password);

    Task AddRoleAsync(string username, string role);

    Task RemoveRoleAsync(string username, string role);
}
