using AuthenticationService.Repository.Entities;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.RoleServiceMethods;

public class CreateRoleAsync : RoleServiceTest
{
    [Test]
    public async Task CreatesNewRole()
    {
        this.roleRepositoryMock
            .Setup(m => m.CreateAsync(It.IsAny<RoleEntity>()))
            .Callback<RoleEntity>(entity => this.roleEntities.Add(entity));

        await this.roleService.CreateRoleAsync("NonExistingRole");

        var createdRole = this.roleEntities.Last();
        Assert.AreEqual("NonExistingRole", createdRole.Role);
        this.roleRepositoryMock.Verify(m => m.CreateAsync(createdRole), Times.Once);
    }

    [Test]
    public void RequiresRoleNotToBeExisting()
    {
        this.roleRepositoryMock
            .Setup(m => m.CreateAsync(It.IsAny<RoleEntity>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<RoleExistenceException>(() => this.roleService.CreateRoleAsync("ExistingRole"));
    }
}
