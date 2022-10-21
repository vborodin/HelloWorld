using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class CreateAsync: UserRepositoryTest
{
    [Test]
    public async Task CreatesUserAndSavesChanges()
    {
        var entity = new UserEntity { Username = "NewUsername" };

        await this.Repository.CreateAsync(entity);

        this.ContextMock.Verify(context => context.AddAsync(entity, default), Times.Once);
        this.ContextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task RequiresUsernameNotToBeTaken()
    {
        var entity = new UserEntity { Username = "ExistingUsername" };
        await this.Repository.CreateAsync(entity);

        Assert.ThrowsAsync<InvalidOperationException>(() => this.Repository.CreateAsync(entity));
    }
}