using System.Text;

using AuthenticationService.Repository.Entities;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class CreateUserAsync: UserServiceTest
{
    [Test]
    public async Task CreatesEntityInRepository()
    {
        var storedEntities = new List<UserEntity>();
        this.saltGeneratorMock
            .Setup(m => m.Generate())
            .Returns("Salt");
        this.hashCalculatorMock
            .Setup(m => m.Calculate(It.IsAny<byte[]>()))
            .Returns((byte[] bytes) => Encoding.UTF8.GetString(bytes) + "Hashed");
        this.repositoryMock
            .Setup(m => m.CreateAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity => { storedEntities.Add(entity); });

        await this.service.CreateUserAsync(
            username: "NewUsername",
            password: "NewPassword",
            email: "NewEmail",
            givenName: "NewGivenName",
            surname: "NewSurname");

        this.saltGeneratorMock.Verify(m => m.Generate(), Times.Once);
        this.hashCalculatorMock.Verify(m => m.Calculate(It.IsAny<byte[]>()), Times.Once);
        this.repositoryMock.Verify(m => m.CreateAsync(It.IsAny<UserEntity>()), Times.Once);
        var entity = storedEntities.Single();
        Assert.AreEqual("NewUsername", entity.Username);
        Assert.AreEqual("NewPasswordSaltPepperHashed", entity.PasswordHash);
        Assert.AreEqual("Salt", entity.Salt);
        Assert.AreEqual("User", entity.Role);
        Assert.AreEqual("NewEmail", entity.Email);
        Assert.AreEqual("NewGivenName", entity.GivenName);
        Assert.AreEqual("NewSurname", entity.Surname);
    }

    [Test]
    public void RequiresUsernameNotToBeTaken()
    {
        this.repositoryMock
            .Setup(m => m.CreateAsync(It.IsAny<UserEntity>()))
            .Throws(new InvalidOperationException());
        this.saltGeneratorMock
            .Setup(m => m.Generate())
            .Returns("Salt");

        Assert.ThrowsAsync<RegistrationException>(() => this.service.CreateUserAsync("TestUsername", "TestPassword"));
    }
}
