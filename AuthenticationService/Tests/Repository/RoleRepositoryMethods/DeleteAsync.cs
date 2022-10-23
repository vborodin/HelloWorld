using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.RoleRepositoryMethods;

public class DeleteAsync: RoleRepositoryTest
{
    [Test]
    public async Task DeletesRoleAndSavesChanges()
    {
        var entity = new RoleEntity() { Id = 1 };
        this.Data.Add(entity);
        this.ContextMock
            .Setup(m => m.Remove(It.IsAny<RoleEntity>()))
            .Callback<RoleEntity>(entity => this.Data.Remove(entity));

        await this.Repository.DeleteAsync(entity);

        this.ContextMock.Verify(m => m.Remove(entity), Times.Once);
        this.ContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        Assert.IsEmpty(this.Data);
    }

    [Test]
    public void RequiresExistingRole()
    {
        var entity = new RoleEntity();

        Assert.ThrowsAsync<InvalidOperationException>(() => this.Repository.DeleteAsync(entity));
    }
}
