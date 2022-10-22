using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Services;
using AuthenticationService.Services.Hashing.HashCalculator;
using AuthenticationService.Services.Hashing.Salt;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

[Category("Services")]
public abstract class UserServiceTest
{
    protected IUserService service = null!;
    protected Mock<IRepository<UserEntity>> userRepositoryMock = null!;
    protected Mock<IRepository<RoleEntity>> roleRepositoryMock = null!;
    protected Mock<IHashCalculator<byte[], string>> hashCalculatorMock = null!;
    protected Mock<ISaltGenerator<string>> saltGeneratorMock = null!;
    protected List<UserEntity> userEntities = null!;
    protected List<RoleEntity> roleEntities = null!;

    [SetUp]
    public void Setup()
    {
        var pepper = "Pepper";
        this.userRepositoryMock = new Mock<IRepository<UserEntity>>();
        this.roleRepositoryMock = new Mock<IRepository<RoleEntity>>();
        this.hashCalculatorMock = new Mock<IHashCalculator<byte[], string>>();
        this.saltGeneratorMock = new Mock<ISaltGenerator<string>>();
        this.userEntities = CreateUserEntities();
        this.roleEntities = CreateRoleEntities();

        this.service = new UserService(
            userRepository: this.userRepositoryMock.Object,
            roleRepository: this.roleRepositoryMock.Object,
            hashCalculator: this.hashCalculatorMock.Object,
            saltGenerator: this.saltGeneratorMock.Object,
            pepper: pepper);
    }

    private List<UserEntity> CreateUserEntities()
    {
        return new()
        {
            new()
            {
                Id = 1,
                Username = "ValidUsername",
                PasswordHash = "ValidHash",
                Salt = "Salt",
                Roles = new List<RoleEntity>()
                {
                    new() { Id = 1, Role = "AssignedRole" }
                }
            }
        };
    }

    private List<RoleEntity> CreateRoleEntities()
    {
        return new List<RoleEntity>()
        {
            new() { Id = 1, Role = "AssignedRole" },
            new() { Id = 2, Role = "NewRole" },
            new() { Id = 3, Role = "User" },
            new() { Id = 4, Role = "NotAssignedRole" }
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
