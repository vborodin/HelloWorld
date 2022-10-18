using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

using Moq;
using Moq.EntityFrameworkCore.DbAsyncQueryProvider;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository.UserRepositoryMethods;

public abstract class UserRepositoryTest
{
    protected UserRepository repository = null!;
    protected Mock<IFilter<UserEntity>> filterMock = null!;
    protected Mock<AppDbContext> contextMock = null!;
    protected Mock<DbSet<UserEntity>> dbSetMock = null!;
    protected List<UserEntity> data = null!;

    [SetUp]
    public void Setup()
    {
        this.data = new List<UserEntity>();
        this.filterMock = CreateFilterMock();
        this.dbSetMock = CreateDbSetMock();
        this.contextMock = CreateAppDbContextContextMock(this.dbSetMock.Object);
        this.repository = new UserRepository(this.contextMock.Object);
    }

    private static Mock<IFilter<UserEntity>> CreateFilterMock()
    {
        var mock = new Mock<IFilter<UserEntity>>();
        mock.Setup(mock => mock
            .Apply(It.IsAny<DbSet<UserEntity>>()))
            .Returns((DbSet<UserEntity> e) => e);
        return mock;
    }

    private Mock<AppDbContext> CreateAppDbContextContextMock(DbSet<UserEntity> dbSet)
    {
        var mock = new Mock<AppDbContext>();
        mock.Setup(m => m.Users).Returns(dbSet);
        mock.Setup(m => m.AddAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Callback<UserEntity, CancellationToken>((entity, _) => this.data.Add(entity));
        mock.Setup(m => m.Update(It.IsAny<UserEntity>()))
            .Callback<UserEntity>((entity) =>
            {
                this.data.RemoveAll(x => x.Id == entity.Id);
                this.data.Add(entity);
            });
        return mock;
    }

    private Mock<DbSet<UserEntity>> CreateDbSetMock()
    {
        var mock = new Mock<DbSet<UserEntity>>();
        var queryable = this.data.AsQueryable();

        mock.As<IQueryable<UserEntity>>()
            .Setup(m => m.Provider)
            .Returns(() => new InMemoryAsyncQueryProvider<UserEntity>(queryable.Provider));
        mock.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        mock.As<IAsyncEnumerable<UserEntity>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(() => new InMemoryDbAsyncEnumerator<UserEntity>(this.data.GetEnumerator()));
        return mock;
    }
}
