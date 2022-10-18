using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class UpdateAsync: UserRepositoryTest
{
    [Test]
    public async Task UpdatesDataAndSavesChanges()
    {
        var entity = new UserEntity
        {
            Id = 1,
            Username = "Username"
        };
        this.data.Add(entity);
        
        await this.repository.UpdateAsync(entity);

        this.contextMock.Verify(context => context.Update(entity), Times.Once);
        this.contextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public void RequiresExistingUser()
    {
        var data = new UserEntity { Id = 1 };
        Assert.ThrowsAsync<InvalidOperationException>(() => this.repository.UpdateAsync(data));
    }
}
