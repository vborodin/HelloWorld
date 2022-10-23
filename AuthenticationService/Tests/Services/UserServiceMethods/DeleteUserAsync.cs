using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.UserServiceMethods;

public class DeleteUserAsync : UserServiceTest
{
    [Test]
    public async Task DeletesUser()
    {
        UserEntity removedEntity = null!;
        this.userRepositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(entity =>
            { 
                this.userEntities.Remove(entity);
                removedEntity = entity;
            });
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns<IFilter<UserEntity>>(filter => ToAsyncEnumerable(filter.Apply(this.userEntities.AsQueryable())));

        await this.service.DeleteUserAsync("ValidUsername");

        Assert.IsEmpty(this.userEntities);
        this.userRepositoryMock.Verify(m => m.DeleteAsync(removedEntity), Times.Once);
        Assert.AreEqual("ValidUsername", removedEntity.Username);
    }

    [Test]
    public void RequiersExistingUser()
    {
        this.userRepositoryMock
            .Setup(m => m.GetAsync(It.IsAny<IFilter<UserEntity>>()))
            .Returns(AsyncEnumerable.Empty<UserEntity>());

        Assert.ThrowsAsync<UserExistenceException>(() => this.service.DeleteUserAsync("NonExistingUser"));
    }
}
