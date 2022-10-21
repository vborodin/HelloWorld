using System.Linq.Expressions;

using AuthenticationService.Repository;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

using Moq;
using Moq.EntityFrameworkCore.DbAsyncQueryProvider;

namespace AuthenticationService.Tests.Repository;

public class TestRepositoryProvider<TEntity> where TEntity : class
{
    public IRepository<TEntity> Repository { get; init; } = null!;
    public Mock<IFilter<TEntity>> FilterMock { get; init; } = null!;
    public Mock<AppDbContext> ContextMock { get; init; } = null!;
    public Mock<DbSet<TEntity>> DbSetMock { get; init; } = null!;
    public List<TEntity> Data { get; init; } = null!;

    public TestRepositoryProvider(Func<AppDbContext, IRepository<TEntity>> repositoryFactory, Expression<Func<AppDbContext, DbSet<TEntity>>> dbSetFactoryExpression)
    {
        this.Data = new List<TEntity>();
        this.FilterMock = CreateFilterMock();
        this.DbSetMock = CreateDbSetMock();
        this.ContextMock = CreateAppDbContextContextMock(dbSetFactoryExpression, this.DbSetMock.Object);
        this.Repository = repositoryFactory(this.ContextMock.Object);
    }

    private static Mock<IFilter<TEntity>> CreateFilterMock()
    {
        var mock = new Mock<IFilter<TEntity>>();
        mock.Setup(mock => mock
            .Apply(It.IsAny<DbSet<TEntity>>()))
            .Returns((DbSet<TEntity> e) => e);
        return mock;
    }

    private Mock<AppDbContext> CreateAppDbContextContextMock(Expression<Func<AppDbContext, DbSet<TEntity>>> dbSetFactoryExpression, DbSet<TEntity> dbSet)
    {
        var mock = new Mock<AppDbContext>();
        mock.Setup(dbSetFactoryExpression).Returns(dbSet);
        mock.Setup(m => m.AddAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
            .Callback<TEntity, CancellationToken>((entity, _) => this.Data.Add(entity));
        mock.Setup(m => m.Update(It.IsAny<TEntity>()))
            .Callback<TEntity>((entity) =>
            {
                this.Data.Add(entity);
            });
        return mock;
    }

    private Mock<DbSet<TEntity>> CreateDbSetMock()
    {
        var mock = new Mock<DbSet<TEntity>>();
        var queryable = this.Data.AsQueryable();

        mock.As<IQueryable<TEntity>>()
            .Setup(m => m.Provider)
            .Returns(() => new InMemoryAsyncQueryProvider<TEntity>(queryable.Provider));
        mock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        mock.As<IAsyncEnumerable<TEntity>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(() => new InMemoryDbAsyncEnumerator<TEntity>(this.Data.GetEnumerator()));
        return mock;
    }
}
