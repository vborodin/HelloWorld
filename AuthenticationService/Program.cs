using System.Text;

using AuthenticationService.Hashing.HashCalculator;
using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Services;
using AuthenticationService.Services.Model;
using AuthenticationService.TokenGenerator;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UserModelContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:AuthenticationDB"]);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer((options) => {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
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
builder.Services.AddScoped<IRepository<UserEntity>, UserModelRepository>();
builder.Services.AddScoped<IUserService>((serviceProvider) =>
{
    return new UserService(
        serviceProvider.GetRequiredService<IRepository<UserEntity>>(),
        serviceProvider.GetRequiredService<IHashCalculator<byte[], string>>(),
        builder.Configuration["Security:Pepper"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
