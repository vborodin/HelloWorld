using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class AddRoleAsync: UserServiceTest
{
    [Test]
    public async Task AssignesNewRole()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(ToAsyncEnumerable(this.userEntities));
        this.userRepositoryMock
            .Setup(m => m.UpdateAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity => this.userEntities.Add(entity));
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns<IFilter<RoleEntity>>(filter => ToAsyncEnumerable(filter.Apply(this.roleEntities.AsQueryable())));

        await this.service.AddRoleAsync(username: "ValidUsername", role: "NewRole");
        
        var before = this.userEntities.First();
        var after = this.userEntities.Last();
        Assert.AreEqual(before.Id, after.Id);
        Assert.AreEqual(before.Username, after.Username);
        Assert.AreEqual(before.PasswordHash, after.PasswordHash);
        Assert.AreEqual(before.Salt, after.Salt);
        Assert.Contains("NewRole", after.Roles.Select(x => x.Role).ToList());
    }

    [Test]
    public void RequiresExistingUsername()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(AsyncEnumerable.Empty<UserEntity>());

        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.AddRoleAsync("InvalidUsername", "NewRole"));
    }

    [Test]
    public void RequiresExistingRole()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns<IFilter<UserEntity>>(filter => ToAsyncEnumerable(filter.Apply(this.userEntities.AsQueryable())));
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns(AsyncEnumerable.Empty<RoleEntity>());

        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.AddRoleAsync("ValidUsername", "NonExistingRole"));
    }
}
