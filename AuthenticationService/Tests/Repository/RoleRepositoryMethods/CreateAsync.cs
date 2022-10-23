using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.RoleRepositoryMethods;

public class CreateAsync: RoleRepositoryTest
{
    [Test]
    public async Task CreatesRoleAndSavesChanges()
    {
        var entity = new RoleEntity() { Role = "NewRole" };

        await this.Repository.CreateAsync(entity);

        this.ContextMock.Verify(context => context.AddAsync(entity, default), Times.Once);
        this.ContextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public void RequiresRoleNotToBeExisting()
    {
        var entity = new RoleEntity() { Role = "Existing" };
        this.Data.Add(entity);

        Assert.ThrowsAsync<InvalidOperationException>(() => this.Repository.CreateAsync(entity));
    }
}
