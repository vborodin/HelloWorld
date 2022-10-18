using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public class GetAsync: UserRepositoryTest
{
    [Test]
    public void AppliesFilterToDbSet()
    {
        this.contextMock
            .Setup(m => m.Users)
            .Returns(this.dbSetMock.Object);

        var result = this.repository.GetAsync(this.filterMock.Object);

        this.filterMock.Verify(filter => filter.Apply(this.dbSetMock.Object), Times.Once);
        Assert.AreEqual(this.dbSetMock.Object, result);
    }
}
