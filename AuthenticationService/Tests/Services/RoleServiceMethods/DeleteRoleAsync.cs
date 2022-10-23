using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.RoleServiceMethods;

public class DeleteRoleAsync : RoleServiceTest
{
    [Test]
    public async Task RemovesRole()
    {
        RoleEntity removedEntity = null!;
        this.roleRepositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<RoleEntity>()))
            .Callback<RoleEntity>(entity =>
            {
                this.roleEntities.Remove(entity);
                removedEntity = entity;
            });
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns<IFilter<RoleEntity>>(filter => ToAsyncEnumerable(filter.Apply(this.roleEntities.AsQueryable())));

        await this.roleService.DeleteRoleAsync("ExistingRole");

        Assert.IsEmpty(this.roleEntities);
        Assert.AreEqual("ExistingRole", removedEntity.Role);
        this.roleRepositoryMock.Verify(m => m.DeleteAsync(removedEntity), Times.Once);
    }

    [Test]
    public void RequiresExistingRole()
    {
        this.roleRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<RoleEntity>>()))
            .Returns(AsyncEnumerable.Empty<RoleEntity>());

        Assert.ThrowsAsync<RoleExistenceException>(() => this.roleService.DeleteRoleAsync("NonExistingRole"));
    }
}
