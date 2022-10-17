using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;

using Microsoft.EntityFrameworkCore;

using Moq;
using Moq.EntityFrameworkCore.DbAsyncQueryProvider;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository;

public class UserRepositoryTest
{
    private UserRepository repository = null!;
    private Mock<IFilter<UserEntity>> filterMock = null!;
    private List<UserEntity> data = null!;
    private Mock<AppDbContext> contextMock = null!;

    [SetUp]
    public void Setup()
    {
        this.filterMock = CreateFilterMock();
        this.data = new List<UserEntity>();
        this.contextMock = CreateAppDbContextContextMock();
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
        var data = new UserEntity
        {
            Id = 1,
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

    [Test]
    public async Task UpdatesData()
    {
        var data = new UserEntity
        {
            Id = 1,
            Username = "username",
            PasswordHash = "hash",
            Salt = "salt",
            Role = "role",
            Email = "email",
            GivenName = "givenname",
            Surname = "surname"
        };
        await this.repository.CreateAsync(data);
        data = new UserEntity
        {
            Id = 1,
            Username = "username1",
            PasswordHash = "hash1",
            Salt = "salt1",
            Role = "role1",
            Email = "email1",
            GivenName = "givenname1",
            Surname = "surname1"
        };
        await this.repository.UpdateAsync(data);

        var updatedData = await this.repository.GetAsync(this.filterMock.Object).SingleAsync();
        Assert.AreEqual(data.Id, updatedData.Id);
        Assert.AreEqual(data.Username, updatedData.Username);
        Assert.AreEqual(data.PasswordHash, updatedData.PasswordHash);
        Assert.AreEqual(data.Salt, updatedData.Salt);
        Assert.AreEqual(data.Role, updatedData.Role);
        Assert.AreEqual(data.Email, updatedData.Email);
        Assert.AreEqual(data.GivenName, updatedData.GivenName);
        Assert.AreEqual(data.Surname, updatedData.Surname);
        this.contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    public void ThrowsInvalidOperationExceptionIfEntityDoesNotExist()
    {
        var data = new UserEntity { Id = 1 };
        Assert.ThrowsAsync<InvalidOperationException>(() => this.repository.UpdateAsync(data));
    }

    private static Mock<IFilter<UserEntity>> CreateFilterMock()
    {
        var mock = new Mock<IFilter<UserEntity>>();
        mock.Setup(mock => mock
            .Apply(It.IsAny<DbSet<UserEntity>>()))
            .Returns((DbSet<UserEntity> e) => e);
        return mock;
    }

    private Mock<AppDbContext> CreateAppDbContextContextMock()
    {
        var mock = new Mock<AppDbContext>();
        var dbSetMock = CreateDbSetMock();
        mock.Setup(m => m.Users).Returns(dbSetMock.Object);
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
