using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class GetUserAsync: UserServiceTest
{
    [Test]
    public async Task ReturnsUserModel()
    {
        this.repositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(ToAsyncEnumerable(this.data));
        this.hashCalculatorMock
            .Setup(m => m.Calculate(It.IsAny<byte[]>()))
            .Returns("ValidHash");

        var result = await this.service.GetUserAsync("ValidUsername", "ValidPassword");

        Assert.AreEqual("ValidUsername", result!.Username);
        Assert.AreEqual("TestEmail", result!.Email);
        Assert.AreEqual("TestRole", result!.Role);
        Assert.AreEqual("TestSurname", result!.Surname);
        Assert.AreEqual("TestGivenName", result!.GivenName);
    }

    [Test]
    public async Task RequiresExistingUsername()
    {
        this.repositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(AsyncEnumerable.Empty<UserEntity>()); 

        var result = await this.service.GetUserAsync("InvalidUsername", "ValidPassword");

        Assert.IsNull(result);
    }

    [Test]
    public async Task RequiresValidPassword()
    {
        this.repositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(ToAsyncEnumerable(this.data));
        this.hashCalculatorMock
            .Setup(m => m.Calculate(It.IsAny<byte[]>()))
            .Returns("InvalidHash");

        var result = await this.service.GetUserAsync("TestUsername", "InvalidPassword");

        Assert.IsNull(result);
    }
}
