using AuthenticationService.Services.Model;

namespace AuthenticationService.Services;

public interface IUserService
{
    UserModel? GetUser(string username, string password);

    void CreateUser(string username, string password, string? email = null, string? givenName = null, string? surname = null);
}
