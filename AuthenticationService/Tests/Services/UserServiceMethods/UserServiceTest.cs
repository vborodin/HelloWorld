using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Services;
using AuthenticationService.Services.Hashing.HashCalculator;
using AuthenticationService.Services.Hashing.Salt;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public abstract class UserServiceTest
{
    protected IUserService service = null!;
    protected Mock<IRepository<UserEntity>> repositoryMock = null!;
    protected Mock<IHashCalculator<byte[], string>> hashCalculatorMock = null!;
    protected Mock<ISaltGenerator<string>> saltGeneratorMock = null!;
    protected List<UserEntity> data = null!;

    [SetUp]
    public void Setup()
    {
        var pepper = "Pepper";
        this.repositoryMock = new Mock<IRepository<UserEntity>>();
        this.hashCalculatorMock = new Mock<IHashCalculator<byte[], string>>();
        this.saltGeneratorMock = new Mock<ISaltGenerator<string>>();
        this.data = CreateTestData();

        this.service = new UserService(
            repository: this.repositoryMock.Object,
            hashCalculator: this.hashCalculatorMock.Object,
            saltGenerator: this.saltGeneratorMock.Object,
            pepper: pepper);
    }

    private List<UserEntity> CreateTestData()
    {
        return new()
        {
            new()
            {
                Id = 0,
                Username = "ValidUsername",
                PasswordHash = "ValidHash",
                Salt = "Salt",
                Email = "TestEmail",
                Role = "TestRole",
                Surname = "TestSurname",
                GivenName = "TestGivenName"
            }
        };
    }

    protected async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            yield return await Task.FromResult(item);
        }
    }
}
