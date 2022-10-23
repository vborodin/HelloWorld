using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.RoleRepositoryMethods;

public class UpdateAsync: RoleRepositoryTest
{
    [Test]
    public async Task UpdatesRoleAndSaveChanges()
    {
        var entity = new RoleEntity() { Id = 1 };
        this.Data.Add(entity);

        await this.Repository.UpdateAsync(entity);

        this.ContextMock.Verify(m => m.Update(entity), Times.Once);
        this.ContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        // Update mock adds entity in the Data without removing old entry
        Assert.AreEqual(2, this.Data.Count);
        Assert.AreEqual(this.Data.Last(), entity);
    }

    [Test]
    public void RequiresExistingRole()
    {
        var entity = new RoleEntity() { Id = 1 };

        Assert.ThrowsAsync<InvalidOperationException>(() => this.Repository.UpdateAsync(entity));
    }
}
