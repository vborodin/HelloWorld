using AuthenticationService.Repository.Entities;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class GetAsync: UserRepositoryTest
{
    [Test]
    public void AppliesFilterToDbSet()
    {
        var entity = new UserEntity() { Id = 1, Username = "TestUsername" };
        this.ContextMock
            .Setup(m => m.Users)
            .Returns(this.DbSetMock.Object);
        this.Entities.Add(entity);

        var result = this.Repository.GetAsync(this.FilterMock.Object);

        this.FilterMock.Verify(filter => filter.Apply(this.DbSetMock.Object), Times.Once);
        Assert.AreEqual(entity, result.SingleAsync().Result);
    }
}
