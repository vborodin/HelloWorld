using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

[Category("Repositories")]
public abstract class UserRepositoryTest
{
    protected IRepository<UserEntity> Repository => this.provider.Repository;
    protected Mock<AppDbContext> ContextMock => this.provider.ContextMock;
    protected Mock<DbSet<UserEntity>> DbSetMock => this.provider.DbSetMock;
    protected List<UserEntity> Data => this.provider.Data;
    protected Mock<IFilter<UserEntity>> FilterMock => this.provider.FilterMock;

    private TestRepositoryProvider<UserEntity> provider = null!; 

    [SetUp]
    public void Setup()
    {
        this.provider = new TestRepositoryProvider<UserEntity>(
            repositoryFactory: context => new UserRepository(context),
            dbSetFactoryExpression: context => context.Users);
    }
}
