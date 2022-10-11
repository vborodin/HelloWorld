using AuthenticationService.Hashing.HashCalculator;
using AuthenticationService.Hashing.Salt;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository;
using AuthenticationService.Services;
using AuthenticationService.Services.Model;
using AuthenticationService.TokenGenerator;

namespace AuthenticationService.Startup;

public static class DependencyInjectionSetup
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddSingleton<ITokenGenerator<UserModel>>((serviceProvider) =>
        {
            var config = builder.Configuration;
            return new UserModelTokenGenerator(
                key: config["Jwt:Key"],
                issuer: config["Jwt:issuer"]);
        });
        builder.Services.AddSingleton<IHashCalculator<byte[], string>>(_ =>
        {
            return new SHA256Base64HashCalculator(int.Parse(builder.Configuration["Security:HashIterations"]));
        });
        builder.Services.AddSingleton<ISaltGenerator<string>>(_ =>
        {
            return new Base64StringSaltGenerator(int.Parse(builder.Configuration["Security:SaltLength"]));
        });
        builder.Services.AddScoped<IRepository<UserEntity>, UserRepository>();
        builder.Services.AddScoped<IUserService>((serviceProvider) =>
        {
            return new UserService(
                serviceProvider.GetRequiredService<IRepository<UserEntity>>(),
                serviceProvider.GetRequiredService<IHashCalculator<byte[], string>>(),
                serviceProvider.GetRequiredService<ISaltGenerator<string>>(),
                builder.Configuration["Security:Pepper"]);
        });
        return builder;
    }
}
