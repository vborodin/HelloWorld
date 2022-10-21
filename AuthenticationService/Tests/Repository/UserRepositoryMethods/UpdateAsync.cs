using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class UpdateAsync: UserRepositoryTest
{
    [Test]
    public async Task UpdatesUserAndSavesChanges()
    {
        var entity = new UserEntity
        {
            Id = 1,
            Username = "Username"
        };
        this.Data.Add(entity);
        
        await this.Repository.UpdateAsync(entity);

        this.ContextMock.Verify(context => context.Update(entity), Times.Once);
        this.ContextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public void RequiresExistingUser()
    {
        var data = new UserEntity { Id = 1 };
        Assert.ThrowsAsync<InvalidOperationException>(() => this.Repository.UpdateAsync(data));
    }
}
