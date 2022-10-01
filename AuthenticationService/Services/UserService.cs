using AuthenticationService.Models;
using AuthenticationService.Repository;
using AuthenticationService.Repository.Filter;

namespace AuthenticationService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public UserModel? GetUser(UserLogin userLogin)
        {
            VerifyUserLogin(userLogin);
            var filter = new UsernamePasswordFilter(userLogin.Username!, userLogin.Password!);
            var result = repository.Get(filter);
            return result.FirstOrDefault();
        }

        private void VerifyUserLogin(UserLogin value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{nameof(UserLogin)} value is null");
            }
            if (value.Username == null || value.Password == null)
            {
                throw new InvalidOperationException($"Required field of {nameof(UserLogin)} is null");
            }
        }
    }
}
