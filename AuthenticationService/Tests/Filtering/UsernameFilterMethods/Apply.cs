using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Entities;

using NUnit.Framework;

namespace AuthenticationService.Tests.Filtering.UsernameFilterMethods;

[Category("Filtering")]
public class Apply
{
    [TestCase("2", 1)]
    [TestCase("1", 2)]
    public void FiltersByUsername(string username, int amount)
    {
        var source = CreateTestData();
        var filter = new UsernameFilter(username);

        var result = filter.Apply(source);

        Assert.AreEqual(amount, result.Count());
        foreach (var user in result)
        {
            Assert.AreEqual(username, user.Username);
        }
    }

    private IQueryable<UserEntity> CreateTestData()
    {
        return new List<UserEntity>
        {
            new() { Username = "1" },
            new() { Username = "" },
            new() { Username = "1" },
            new() { Username = "2" },
            new() { Username = null! }
        }.AsQueryable();
    }
}
