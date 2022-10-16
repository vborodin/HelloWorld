using System.Text;

using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services;
using AuthenticationService.Services.Exceptions;
using AuthenticationService.Services.Hashing.HashCalculator;
using AuthenticationService.Services.Hashing.Salt;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services;

public class UserServiceTest
{
    private IUserService service = null!;
    private Mock<IRepository<UserEntity>> repositoryMock = null!;
    private Mock<IHashCalculator<byte[], string>> hashCalculatorMock = null!;
    private Mock<ISaltGenerator<string>> saltGeneratorMock = null!;
    private List<UserEntity> data = null!;

    [SetUp]
    public void Setup()
    {
        var pepper = "Pepper";
        this.repositoryMock = CreateRepositoryMock();
        this.hashCalculatorMock = CreateHashCalculatorMock();
        this.saltGeneratorMock = CreateSaltGeneratorMock();
        this.data = CreateTestData();

        this.service = new UserService(
            repository: this.repositoryMock.Object,
            hashCalculator: this.hashCalculatorMock.Object,
            saltGenerator: this.saltGeneratorMock.Object,
            pepper: pepper);
    }

    [Test]
    public async Task ReturnsUserByUsernameAndPassword()
    {
        var result = await this.service.GetUserAsync("TestUsername", "TestPassword");

        Assert.AreEqual("TestUsername", result!.Username);
        Assert.AreEqual("TestEmail", result!.Email);
        Assert.AreEqual("TestRole", result!.Role);
        Assert.AreEqual("TestSurname", result!.Surname);
        Assert.AreEqual("TestGivenName", result!.GivenName);
    }

    [Test]
    public async Task ReturnsNullForInvalidUsername()
    {
        var result = await this.service.GetUserAsync("InvalidUsername", "TestPassword");

        Assert.IsNull(result);
    }

    [Test]
    public async Task ReturnsNullForInvalidPassword()
    {
        var result = await this.service.GetUserAsync("TestUsername", "InvalidPassword");

        Assert.IsNull(result);
    }

    [Test]
    public async Task CreatesUserFromUserModelAndPassword()
    {
        var before = await this.service.GetUserAsync("NewUsername", "NewPassword");
        Assert.IsNull(before);

        await this.service.CreateUserAsync(
            username: "NewUsername",
            password: "NewPassword",
            email: "NewEmail",
            givenName: "NewGivenName",
            surname: "NewSurname");

        var result = await this.service.GetUserAsync("NewUsername", "NewPassword");

        Assert.AreEqual("NewUsername", result!.Username);
        Assert.AreEqual("User", result!.Role);
        Assert.AreEqual("NewEmail", result!.Email);
        Assert.AreEqual("NewGivenName", result!.GivenName);
        Assert.AreEqual("NewSurname", result!.Surname);
    }

    [Test]
    public async Task SetsRoleForExistingUser()
    {
        await this.service.SetRoleAsync(username: "TestUsername", role: "NewRole");
        var user = await this.service.GetUserAsync(username: "TestUsername", password: "TestPassword");

        Assert.AreEqual("NewRole", user!.Role);
    }

    [Test]
    public void ThrowsInvalidOperationExceptionWhenSetsRoleForNonexistingUser()
    {
        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.SetRoleAsync("nonexisting", "role"));
    }

    private Mock<ISaltGenerator<string>> CreateSaltGeneratorMock()
    {
        var mock = new Mock<ISaltGenerator<string>>();
        mock.Setup(m => m.Generate()).Returns("TestSalt");
        return mock;
    }

    private Mock<IHashCalculator<byte[], string>> CreateHashCalculatorMock()
    {
        var mock = new Mock<IHashCalculator<byte[], string>>();
        mock.Setup(m => m.Calculate(It.IsAny<byte[]>()))
            .Returns((byte[] bytes) => Encoding.UTF8.GetString(bytes) + "Hashed");
        return mock;
    }

    private Mock<IRepository<UserEntity>> CreateRepositoryMock()
    {
        var mock = new Mock<IRepository<UserEntity>>();
        mock.Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns((IFilter<UserEntity> filter) => ToAsyncEnumerable(filter.Apply(this.data.AsQueryable())));
        mock.Setup(m => m.CreateAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(e => this.data.Add(e));
        return mock;
    }

    private async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            yield return await Task.FromResult(item);
        }
    }

    private List<UserEntity> CreateTestData()
    {
        return new List<UserEntity>
        {
            new()
            {
                Email = "TestEmail",
                GivenName = "TestGivenName",
                Id = 0,
                PasswordHash = "TestPasswordTestSaltPepperHashed",
                Role = "TestRole",
                Salt = "TestSalt",
                Surname = "TestSurname",
                Username = "TestUsername"
            }
        };
    }
}
