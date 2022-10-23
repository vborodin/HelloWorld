using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using NUnit.Framework;

namespace AuthenticationService.Tests.Filtering.RoleFilterMethods;

[Category("Filtering")]
public class Apply
{
    [TestCase("1", 1)]
    [TestCase("2", 2)]
    public void FiltersByRole(string role, int amount)
    {
        var source = CreateTestData();
        var filter = new RoleFilter(role);

        var result = filter.Apply(source);

        Assert.AreEqual(amount, result.Count());
        foreach (var r in result)
        {
            Assert.AreEqual(role, r.Role);
        }
    }

    private IQueryable<RoleEntity> CreateTestData()
    {
        return new List<RoleEntity>
        {
            new() { Role = "1" },
            new() { Role = "2" },
            new() { Role = "3" },
            new() { Role = "2" }
        }.AsQueryable();
    }
}
