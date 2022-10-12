using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Hashing.HashCalculator;
using AuthenticationService.Services.Hashing.HashingData;
using AuthenticationService.Services.Hashing.Salt;
using AuthenticationService.Services.Model;

namespace AuthenticationService.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserEntity> repository;
    private readonly IHashCalculator<byte[], string> hashCalculator;
    private readonly ISaltGenerator<string> saltGenerator;
    private readonly string pepper;

    public UserService(IRepository<UserEntity> repository, IHashCalculator<byte[], string> hashCalculator, ISaltGenerator<string> saltGenerator, string pepper)
    {
        this.repository = repository;
        this.hashCalculator = hashCalculator;
        this.saltGenerator = saltGenerator;
        this.pepper = pepper;
    }

    public async Task CreateUserAsync(string username, string password, string? email = null, string? givenName = null, string? surname = null)
    {
        var salt = this.saltGenerator.Generate();
        var hashingData = new SaltPepperUTF8HashingData(password, salt, this.pepper);
        var hash = this.hashCalculator.Calculate(hashingData.Data);

        var data = new UserEntity()
        {
            Email = email,
            GivenName = givenName,
            PasswordHash = hash,
            Role = "User",
            Salt = salt,
            Surname = surname,
            Username = username
        };

        await this.repository.CreateAsync(data);
    }

    public async Task<UserModel?> GetUserAsync(string username, string password)
    {
        var filter = new UsernameFilter(username);
        var userEntity = await this.repository.GetAsync(filter).FirstOrDefaultAsync();
        if (userEntity != null && IsPasswordValid(password, userEntity.PasswordHash, userEntity.Salt))
        {
            return new UserModel(
                Username: userEntity.Username,
                Email: userEntity.Email,
                Role: userEntity.Role,
                Surname: userEntity.Surname,
                GivenName: userEntity.GivenName);
        }
        return null;
    }

    private bool IsPasswordValid(string password, string passwordHash, string salt)
    {
        var hashingData = new SaltPepperUTF8HashingData(password, salt, this.pepper);
        var hash = this.hashCalculator.Calculate(hashingData.Data);
        return hash == passwordHash;
    }
}
