using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

namespace AuthenticationService.Repository;

public interface IUserRepository
{
    public IEnumerable<UserModel> Get(IFilter<UserModel> filter);
}
