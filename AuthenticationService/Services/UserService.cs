using AuthenticationService.Hashing.HashCalculator;
using AuthenticationService.Hashing.HashingData;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

namespace AuthenticationService.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserModel> repository;
    private readonly IHashCalculator<byte[], string> hashCalculator;
    private readonly string pepper;

    public UserService(IRepository<UserModel> repository, IHashCalculator<byte[], string> hashCalculator, string pepper)
    {
        this.repository = repository;
        this.hashCalculator = hashCalculator;
        this.pepper = pepper;
    }

    public UserModel? GetUser(UserLogin userLogin)
    {
        VerifyUserLogin(userLogin);
        var filter = new UsernameFilter(userLogin.Username);
        var user = this.repository.Get(filter).FirstOrDefault();
        if (user != null && IsPasswordValid(userLogin.Password, user.PasswordHash, user.Salt))
        {
            return user;
        }
        return null;
    }

    private bool IsPasswordValid(string password, string passwordHash, string salt)
    {
        var hashingData = new SaltPepperUTF8HashingData(password, salt, this.pepper);
        var hash = this.hashCalculator.Calculate(hashingData.Data);
        return hash == passwordHash;
    }

    private void VerifyUserLogin(UserLogin value)
    {
        _ = value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            { Username: var u } when u == null => throw new InvalidOperationException($"Required field {nameof(UserLogin.Username)} of {nameof(UserLogin)} is null"),
            { Password: var p } when p == null => throw new InvalidOperationException($"Required field {nameof(UserLogin.Password)} of {nameof(UserLogin)} is null"),
            _ => value
        };
    }
}
