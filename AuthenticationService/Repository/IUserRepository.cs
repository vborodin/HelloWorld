using AuthenticationService.Models;
using AuthenticationService.Repository.Filter;

namespace AuthenticationService.Repository
{
    public interface IUserRepository
    {
        public IEnumerable<UserModel> Get(IFilter<UserModel> filter);
    }
}
