using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class CreateAsync: UserRepositoryTest
{
    [Test]
    public async Task AddsDataAndSavesChanges()
    {
        var entity = new UserEntity { Username = "NewUsername" };

        await this.repository.CreateAsync(entity);

        this.contextMock.Verify(context => context.AddAsync(entity, default), Times.Once);
        this.contextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task RequiresUsernameNotToBeTaken()
    {
        var entity = new UserEntity { Username = "ExistingUsername" };
        await this.repository.CreateAsync(entity);

        Assert.ThrowsAsync<InvalidOperationException>(() => this.repository.CreateAsync(entity));
    }
}