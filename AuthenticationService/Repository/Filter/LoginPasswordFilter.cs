using AuthenticationService.Models;

namespace AuthenticationService.Repository.Filter
{
    public class UsernamePasswordFilter : IFilter<UserModel>
    {
        private readonly string username;
        private readonly string passwordHash;

        public UsernamePasswordFilter(string username, string passwordHash)
        {
            this.username = username;
            this.passwordHash = passwordHash;
        }

        public IEnumerable<UserModel> Apply(IEnumerable<UserModel> source)
        {
            return source.Where(x => x.Username?.ToLower() == username?.ToLower() && x.PasswordHash == passwordHash);
        }
    }
}
