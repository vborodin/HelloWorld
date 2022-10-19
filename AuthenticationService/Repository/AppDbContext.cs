using AuthenticationService.Repository.Entities;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public AppDbContext() { }

    public virtual DbSet<UserEntity> Users { get; set; } = null!;
}
