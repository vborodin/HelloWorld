using AuthenticationService.Repository;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Entities;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository;

public class UserRepositoryTest
{
    private UserRepository repository = null!;
    private Mock<IFilter<UserEntity>> filterMock = null!;
    private List<UserEntity> data = null!;
    private Mock<DbSet<UserEntity>> dbSetMock = null!;
    private Mock<UserModelContext> contextMock = null!;

    [SetUp]
    public void Setup()
    {
        this.filterMock = CreateFilterMock();
        this.data = new List<UserEntity>();
        this.dbSetMock = CreateDbSetMock();
        this.contextMock = CreateDataContextMock(this.dbSetMock.Object);
        this.repository = new UserRepository(this.contextMock.Object);
    }

    [Test]
    public void AppliesFilterWhenGetsData()
    {
        var result = this.repository.Get(this.filterMock.Object);

        this.filterMock.Verify(filter => filter.Apply(result), Times.Once);
    }

    [Test]
    public void CreatesData()
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
        
        this.repository.Create(data);

        var createdData = this.repository.Get(this.filterMock.Object).Single();
        Assert.AreEqual(data.Id, createdData.Id);
        Assert.AreEqual(data.Username, createdData.Username);
        Assert.AreEqual(data.PasswordHash, createdData.PasswordHash);
        Assert.AreEqual(data.Salt, createdData.Salt);
        Assert.AreEqual(data.Role, createdData.Role);
        Assert.AreEqual(data.Email, createdData.Email);
        Assert.AreEqual(data.GivenName, createdData.GivenName);
        Assert.AreEqual(data.Surname, createdData.Surname);
        this.contextMock.Verify(context => context.SaveChanges(), Times.Once);
    }

    private static Mock<IFilter<UserEntity>> CreateFilterMock()
    {
        var mock = new Mock<IFilter<UserEntity>>();
        mock.Setup(mock => mock
            .Apply(It.IsAny<IEnumerable<UserEntity>>()))
            .Returns((IEnumerable<UserEntity> e) => e);
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
        var queryable = this.data.AsQueryable();
        var mock = new Mock<DbSet<UserEntity>>();
        mock.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mock.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        mock.Setup(m => m.Add(It.IsAny<UserEntity>())).Callback<UserEntity>(s => this.data.Add(s));
        return mock;
    }
}
