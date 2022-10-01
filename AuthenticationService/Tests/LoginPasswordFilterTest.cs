using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

using NUnit.Framework;

namespace AuthenticationService.Tests;

public class UsernamePasswordFilterTest
{
    private readonly IEnumerable<UserModel> source = CreateTestData();

    [Test]
    public void ReturnsCorrectUserModel()
    {
        var filter = new UsernamePasswordFilter("1", "1");
        var result = filter.Apply(this.source);
        var match = result.Single();
        Assert.True(match.Username == "1" && match.PasswordHash == "1");
    }

    [Test]
    public void ReturnsAllUserModels()
    {
        var filter = new UsernamePasswordFilter("2", "2");
        var result = filter.Apply(this.source);
        Assert.True(result.Count() == 2);
    }

    [Test]
    public void ReturnsNothingForIncorrectLogin()
    {
        var filter = new UsernamePasswordFilter("nonexistent", "1");
        var result = filter.Apply(this.source);
        Assert.IsEmpty(result);
    }

    [Test]
    public void ReturnsNothingForIncorrectPassword()
    {
        var filter = new UsernamePasswordFilter("1", "nonexistent");
        var result = filter.Apply(this.source);
        Assert.IsEmpty(result);
    }

    [Test]
    public void UsernameIsCaseInsensitive()
    {
        var filter = new UsernamePasswordFilter("john", "2");
        var result = filter.Apply(this.source);
        Assert.True(result.Count() == 1);
    }

    private static IEnumerable<UserModel> CreateTestData()
    {
        return new List<UserModel> {
            new () { Username = "1", PasswordHash = "1" },
            new () { Username = "1", PasswordHash = "2" },
            new () { Username = "2", PasswordHash = "2" },
            new () { Username = "2", PasswordHash = "2" },
            new () { Username = "John", PasswordHash = "2" },
            new () { Username = null, PasswordHash = "1" },
            new () { Username = "1", PasswordHash = null },
            new () { Username = null, PasswordHash = null }
        };
    }
}
