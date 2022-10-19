using AuthenticationService.Repository;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Startup;

public static class DatabaseSetup
{
    public static WebApplicationBuilder RegisterDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["ConnectionStrings:AuthenticationDB"]);
        });
        return builder;
    }
}
