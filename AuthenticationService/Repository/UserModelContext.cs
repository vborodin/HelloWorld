using AuthenticationService.Repository.Model;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class UserModelContext: DbContext
{
    public UserModelContext(DbContextOptions<UserModelContext> options) : base(options) { }
    public UserModelContext() { }

    public virtual DbSet<UserModel> Users { get; set; } = null!;
}
