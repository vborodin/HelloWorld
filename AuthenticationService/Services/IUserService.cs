using AuthenticationService.Services.Model;

namespace AuthenticationService.Services;

public interface IUserService
{
    UserModel? GetUser(string username, string password);

    void CreateUser(UserModel user);
}
