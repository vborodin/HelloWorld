using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

using NUnit.Framework;

namespace AuthenticationService.Tests.Filtering;

public class UsernameFilterTest
{
    [Test]
    public void ReturnsUserModelByUsername()
    {
        var source = CreateTestData();
        var filter = new UsernameFilter("2");

        var result = filter.Apply(source);

        Assert.AreEqual("2", result.Single().Username);
    }

    [Test]
    public void ReturnsAllMatches()
    {
        var source = CreateTestData();
        var filter = new UsernameFilter("1");

        var result = filter.Apply(source);

        Assert.AreEqual(2, result.Count());
    }

    private IEnumerable<UserModel> CreateTestData()
    {
        return new List<UserModel>
        {
            new() { Username = "1" },
            new() { Username = "" },
            new() { Username = "1" },
            new() { Username = "2" },
            new() { Username = null }
        };
    }
}
