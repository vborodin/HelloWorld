using AuthenticationService.Repository.Model;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class UserModelContext: DbContext
{
    public virtual DbSet<UserModel> Users { get; set; } = null!;
}
