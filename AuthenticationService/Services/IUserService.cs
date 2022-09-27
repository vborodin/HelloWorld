using AuthenticationService.Models;

namespace AuthenticationService.Services
{
    public interface IUserService
    {
        UserModel? GetUser(UserLogin userLogin);
    }
}
