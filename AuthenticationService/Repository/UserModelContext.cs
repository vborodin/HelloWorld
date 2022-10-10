using AuthenticationService.Repository.Entities;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class UserModelContext: DbContext
{
    public UserModelContext(DbContextOptions<UserModelContext> options) : base(options) { }
    public UserModelContext() { }

    public virtual DbSet<UserEntity> Users { get; set; } = null!;
}
