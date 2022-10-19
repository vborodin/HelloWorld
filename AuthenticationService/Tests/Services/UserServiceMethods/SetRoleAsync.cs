using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class SetRoleAsync: UserServiceTest
{
    [Test]
    public async Task UpdatesRole()
    {
        this.repositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(ToAsyncEnumerable(this.data));
        this.repositoryMock
            .Setup(m => m.UpdateAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity => this.data.Add(entity));

        await this.service.SetRoleAsync(username: "ValidUsername", role: "NewRole");
        
        var before = this.data.First();
        var after = this.data.Last();
        Assert.AreEqual(before.Id, after.Id);
        Assert.AreEqual(before.Username, after.Username);
        Assert.AreEqual(before.PasswordHash, after.PasswordHash);
        Assert.AreEqual(before.Salt, after.Salt);
        Assert.AreEqual(before.Email, after.Email);
        Assert.AreEqual("NewRole", after.Role);
        Assert.AreEqual(before.Surname, after.Surname);
        Assert.AreEqual(before.GivenName, after.GivenName);
    }

    [Test]
    public void RequiresExistingUsername()
    {
        this.repositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(AsyncEnumerable.Empty<UserEntity>());

        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.SetRoleAsync("InvalidUsername", "NewRole"));
    }
}
