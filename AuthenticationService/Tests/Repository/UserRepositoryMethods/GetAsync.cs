using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class GetAsync: UserRepositoryTest
{
    [Test]
    public void AppliesFilterToDbSet()
    {
        this.ContextMock
            .Setup(m => m.Users)
            .Returns(this.DbSetMock.Object);

        var result = this.Repository.GetAsync(this.FilterMock.Object);

        this.FilterMock.Verify(filter => filter.Apply(this.DbSetMock.Object), Times.Once);
        Assert.AreEqual(this.DbSetMock.Object, result);
    }
}
