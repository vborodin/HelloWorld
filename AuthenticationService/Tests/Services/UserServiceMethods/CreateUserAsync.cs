using System.Text;

using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class CreateUserAsync: UserServiceTest
{
    [Test]
    public async Task CreatesEntityInRepository()
    {
        this.saltGeneratorMock
            .Setup(m => m.Generate())
            .Returns("Salt");
        this.hashCalculatorMock
            .Setup(m => m.Calculate(It.IsAny<byte[]>()))
            .Returns((byte[] bytes) => Encoding.UTF8.GetString(bytes) + "Hashed");
        this.userRepositoryMock
            .Setup(m => m.CreateAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity => { this.userEntities.Add(entity); });
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns(ToAsyncEnumerable(this.roleEntities.Where(x => x.Role == "User")));

        await this.service.CreateUserAsync(
            username: "NewUsername",
            password: "NewPassword");

        this.saltGeneratorMock.Verify(m => m.Generate(), Times.Once);
        this.hashCalculatorMock.Verify(m => m.Calculate(It.IsAny<byte[]>()), Times.Once);
        this.userRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<UserEntity>()), Times.Once);
        var entity = this.userEntities.Last();
        Assert.AreEqual("NewUsername", entity.Username);
        Assert.AreEqual("NewPasswordSaltPepperHashed", entity.PasswordHash);
        Assert.AreEqual("Salt", entity.Salt);
        Assert.AreEqual("User", entity.Roles.Select(x => x.Role).Single());
    }

    [Test]
    public void RequiresUsernameNotToBeTaken()
    {
        this.userRepositoryMock
            .Setup(m => m.CreateAsync(It.IsAny<UserEntity>()))
            .Throws(new InvalidOperationException());
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns(ToAsyncEnumerable(this.roleEntities.Where(x => x.Role == "User")));
        this.saltGeneratorMock
            .Setup(m => m.Generate())
            .Returns("Salt");

        Assert.ThrowsAsync<RegistrationException>(() => this.service.CreateUserAsync("TestUsername", "TestPassword"));
    }

    [Test]
    public void RequiresDefaultRoleToExist()
    {
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns(AsyncEnumerable.Empty<RoleEntity>());

        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.CreateUserAsync("TestUsername", "TestPassword"));
    }
}
