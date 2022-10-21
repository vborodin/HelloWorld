using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.RoleRepositoryMethods;

public abstract class RoleRepositoryTest
{
    protected IRepository<RoleEntity> Repository => this.provider.Repository;
    protected Mock<AppDbContext> ContextMock => this.provider.ContextMock;
    protected Mock<DbSet<RoleEntity>> DbSetMock => this.provider.DbSetMock;
    protected List<RoleEntity> Data => this.provider.Data;
    protected Mock<IFilter<RoleEntity>> FilterMock => this.provider.FilterMock;

    private TestRepositoryProvider<RoleEntity> provider = null!;

    [SetUp]
    public void Setup()
    {
        this.provider = new TestRepositoryProvider<RoleEntity>(
            repositoryFactory: context => new RoleRepository(context),
            dbSetFactoryExpression: context => context.Roles);
    }
}
