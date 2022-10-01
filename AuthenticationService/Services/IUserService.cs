using AuthenticationService.Models;
using AuthenticationService.Repository.Model;

namespace AuthenticationService.Services;

public interface IUserService
{
    UserModel? GetUser(UserLogin userLogin);
}
