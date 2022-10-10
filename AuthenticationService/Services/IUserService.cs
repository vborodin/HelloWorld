using AuthenticationService.Repository.Model;

namespace AuthenticationService.Services;

public interface IUserService
{
    UserModel? GetUser(string username, string password);
}
