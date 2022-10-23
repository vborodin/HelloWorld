using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class DeleteAsync: UserRepositoryTest
{
    [Test]
    public async Task DeletesUserAndSavesChanges()
    {
        var entity = new UserEntity() { Id = 1 };
        this.Entities.Add(entity);
        this.ContextMock
            .Setup(m => m.Remove(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity => this.Entities.Remove(entity));
        
        await this.Repository.DeleteAsync(entity);

        this.ContextMock.Verify(m => m.Remove(entity), Times.Once);
        this.ContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        Assert.IsEmpty(this.Entities);
    }

    [Test]
    public void RequiresExistingUser()
    {
        var entity = new UserEntity() { Id = 1 };

        Assert.ThrowsAsync<InvalidOperationException>(() => this.Repository.DeleteAsync(entity));
    }
}
