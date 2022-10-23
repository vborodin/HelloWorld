using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Services;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Services.RoleServiceMethods;

[Category("Services")]
public abstract class RoleServiceTest
{
    protected RoleService roleService = null!;
    protected Mock<IRepository<RoleEntity>> roleRepositoryMock = null!;
    protected List<RoleEntity> roleEntities = null!;

    [SetUp]
    public void Setup()
    {
        this.roleRepositoryMock = new Mock<IRepository<RoleEntity>>();
        this.roleService = new RoleService(this.roleRepositoryMock.Object);
        this.roleEntities = CreateRoleEntities();
    }

    protected async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            yield return await Task.FromResult(item);
        }
    }

    private List<RoleEntity> CreateRoleEntities()
    {
        return new List<RoleEntity>
        {
            new() { Id = 1, Role = "ExistingRole" }
        };
    }
}
