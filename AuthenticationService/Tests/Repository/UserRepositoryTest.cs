using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository;

public class UserRepositoryTest
{
    private UserRepository repository = null!;
    private Mock<IFilter<UserEntity>> filterMock = null!;
    private List<UserEntity> data = null!;
    private IAsyncEnumerable<UserEntity> enumerable = null!;
    private IQueryable<UserEntity> queryable = null!;
    private Mock<DbSet<UserEntity>> dbSetMock = null!;
    private Mock<UserModelContext> contextMock = null!;

    [SetUp]
    public void Setup()
    {
        this.filterMock = CreateFilterMock();
        this.data = new List<UserEntity>();
        this.enumerable = ToAsyncEnumerable(this.data);
        this.queryable = this.data.AsQueryable();
        this.dbSetMock = CreateDbSetMock();
        this.contextMock = CreateDataContextMock(this.dbSetMock.Object);
        this.repository = new UserRepository(this.contextMock.Object);
    }

    

    [Test]
    public void AppliesFilterWhenGetsData()
    {
        var result = this.repository.GetAsync(this.filterMock.Object);

        this.filterMock.Verify(filter => filter.Apply(It.IsAny<IQueryable<UserEntity>>()), Times.Once);
    }

    [Test]
    public async Task CreatesData()
    {
        var data = new UserEntity()
        {
            Id = 0,
            Username = "username",
            PasswordHash = "hash",
            Salt = "salt",
            Role = "role",
            Email = "email",
            GivenName = "givenname",
            Surname = "surname"
        };
        
        await this.repository.CreateAsync(data);

        var createdData = await this.repository.GetAsync(this.filterMock.Object).SingleAsync();
        Assert.AreEqual(data.Id, createdData.Id);
        Assert.AreEqual(data.Username, createdData.Username);
        Assert.AreEqual(data.PasswordHash, createdData.PasswordHash);
        Assert.AreEqual(data.Salt, createdData.Salt);
        Assert.AreEqual(data.Role, createdData.Role);
        Assert.AreEqual(data.Email, createdData.Email);
        Assert.AreEqual(data.GivenName, createdData.GivenName);
        Assert.AreEqual(data.Surname, createdData.Surname);
        this.contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Mock<IFilter<UserEntity>> CreateFilterMock()
    {
        var mock = new Mock<IFilter<UserEntity>>();
        mock.Setup(mock => mock
            .Apply(It.IsAny<DbSet<UserEntity>>()))
            .Returns((DbSet<UserEntity> e) => e);
        return mock;
    }

    private static Mock<UserModelContext> CreateDataContextMock(DbSet<UserEntity> dbSet)
    {
        var mock = new Mock<UserModelContext>();
        mock.Setup(m => m.Users).Returns(dbSet);
        return mock;
    }

    private Mock<DbSet<UserEntity>> CreateDbSetMock()
    {
        var mock = new Mock<DbSet<UserEntity>>();
        
        mock.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(this.queryable.Provider);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(this.queryable.Expression);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(this.queryable.ElementType);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(() => this.queryable.GetEnumerator());

        mock.As<IAsyncEnumerable<UserEntity>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(() => ToAsyncEnumerable(this.data).GetAsyncEnumerator());

        mock.Setup(m => m.AddAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>())).Callback<UserEntity, CancellationToken>((s, _) => this.data.Add(s));
        return mock;
    }

    private async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            yield return await Task.FromResult(item);
        }
    }
}
