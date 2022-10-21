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
        this.data.Add(entity);
        this.contextMock
            .Setup(m => m.Remove<UserEntity>(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity => this.data.Remove(entity));
        
        await this.repository.DeleteAsync(entity);

        this.contextMock.Verify(m => m.Remove(entity), Times.Once);
        this.contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        Assert.IsEmpty(this.data);
    }

    [Test]
    public async Task RequiresExistingUser()
    {
        var entity = new UserEntity() { Id = 1 };

        Assert.ThrowsAsync<InvalidOperationException>(() => this.repository.DeleteAsync(entity));
    }
}
