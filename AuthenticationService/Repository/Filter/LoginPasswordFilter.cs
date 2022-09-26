using AuthenticationService.Models;

namespace AuthenticationService.Repository.Filter
{
    public class UsernamePasswordFilter : IFilter<UserModel>
    {
        private readonly string username;
        private readonly string password;

        public UsernamePasswordFilter(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public IEnumerable<UserModel> Filter(IEnumerable<UserModel> source)
        {
            return source.Where(x => x.Username == username && x.Password == password);
        }
    }
}
