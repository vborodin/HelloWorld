using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class RemoveRoleAsync : UserServiceTest
{
    [Test]
    public async Task RemovesRole()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns<IFilter<UserEntity>>(filter => ToAsyncEnumerable(filter.Apply(this.userEntities.AsQueryable())));

        await this.service.RemoveRoleAsync("ValidUsername", "AssignedRole");

        var userEntity = this.userEntities.First();
        Assert.IsEmpty(userEntity!.Roles);
        this.userRepositoryMock.Verify(m => m.UpdateAsync(userEntity), Times.Once);
    }

    [Test]
    public void RequiresExistingUsername()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(AsyncEnumerable.Empty<UserEntity>());

        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.RemoveRoleAsync("NonExistingUsername", "ExistingRole"));
    }

    [Test]
    public void RequiersRoleToBeAssigned()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns<IFilter<UserEntity>>(filter => ToAsyncEnumerable(filter.Apply(this.userEntities.AsQueryable())));

        Assert.ThrowsAsync<RoleAssignmentException>(() => this.service.RemoveRoleAsync("ValidUsername", "NotAssignedRole"));
    }
}
