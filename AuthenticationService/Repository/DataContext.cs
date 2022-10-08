using AuthenticationService.Repository.Model;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<UserModel> Users { get; set; } = null!;
}
