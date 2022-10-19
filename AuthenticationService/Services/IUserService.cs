using AuthenticationService.Services.Model;

namespace AuthenticationService.Services;

public interface IUserService
{
    Task<UserModel?> GetUserAsync(string username, string password);

    Task CreateUserAsync(string username, string password, string? email = null, string? givenName = null, string? surname = null);

    Task SetRoleAsync(string username, string role);
}
