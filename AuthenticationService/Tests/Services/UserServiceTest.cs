using System.Text;

using AuthenticationService.Hashing.HashCalculator;
using AuthenticationService.Hashing.Salt;
using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services;
using AuthenticationService.Services.Model;

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
    public void ReturnsUserByUsernameAndPassword()
    {
        var result = this.service.GetUser("TestUsername", "TestPassword");

        Assert.AreEqual("TestUsername", result!.Username);
        Assert.AreEqual("TestEmail", result!.Email);
        Assert.AreEqual("TestRole", result!.Role);
        Assert.AreEqual("TestSurname", result!.Surname);
        Assert.AreEqual("TestGivenName", result!.GivenName);
    }

    [Test]
    public void ReturnsNullForInvalidUsername()
    {
        var result = this.service.GetUser("InvalidUsername", "TestPassword");

        Assert.IsNull(result);
    }

    [Test]
    public void ReturnsNullForInvalidPassword()
    {
        var result = this.service.GetUser("TestUsername", "InvalidPassword");

        Assert.IsNull(result);
    }

    [Test]
    public void CreatesUserFromUserModelAndPassword()
    {
        var before = this.service.GetUser("NewUsername", "NewPassword");
        Assert.IsNull(before);

        this.service.CreateUser(
            username: "NewUsername",
            password: "NewPassword",
            email: "NewEmail",
            givenName: "NewGivenName",
            surname: "NewSurname");

        var result = this.service.GetUser("NewUsername", "NewPassword");

        Assert.AreEqual("NewUsername", result!.Username);
        Assert.AreEqual("User", result!.Role);
        Assert.AreEqual("NewEmail", result!.Email);
        Assert.AreEqual("NewGivenName", result!.GivenName);
        Assert.AreEqual("NewSurname", result!.Surname);
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
        mock.Setup(m => m.Get(It.IsAny<IFilter<UserEntity>>()))
            .Returns((IFilter<UserEntity> filter) => filter.Apply(this.data));
        mock.Setup(m => m.Create(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(e => this.data.Add(e));
        return mock;
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
