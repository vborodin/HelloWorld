using System.Reflection.Metadata;

using AuthenticationService.Repository;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Repository.Model;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Repository;

public class UserModelRepositoryTest
{
    private UserModelRepository repository = null!;
    private Mock<IFilter<UserModel>> filterMock = null!;
    private List<UserModel> data = null!;
    private Mock<DbSet<UserModel>> dbSetMock = null!;
    private Mock<UserModelContext> contextMock = null!;

    [SetUp]
    public void Setup()
    {
        this.filterMock = CreateFilterMock();
        this.data = new List<UserModel>();
        this.dbSetMock = CreateDbSetMock();
        this.contextMock = CreateDataContextMock(this.dbSetMock.Object);
        this.repository = new UserModelRepository(this.contextMock.Object);
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
        var data = new UserModel()
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

    private static Mock<IFilter<UserModel>> CreateFilterMock()
    {
        var mock = new Mock<IFilter<UserModel>>();
        mock.Setup(mock => mock
            .Apply(It.IsAny<IEnumerable<UserModel>>()))
            .Returns((IEnumerable<UserModel> e) => e);
        return mock;
    }

    private static Mock<UserModelContext> CreateDataContextMock(DbSet<UserModel> dbSet)
    {
        var mock = new Mock<UserModelContext>();
        mock.Setup(m => m.Users).Returns(dbSet);
        return mock;
    }

    private Mock<DbSet<UserModel>> CreateDbSetMock()
    {
        var queryable = this.data.AsQueryable();
        var mock = new Mock<DbSet<UserModel>>();
        mock.As<IQueryable<UserModel>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mock.As<IQueryable<UserModel>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mock.As<IQueryable<UserModel>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mock.As<IQueryable<UserModel>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        mock.Setup(m => m.Add(It.IsAny<UserModel>())).Callback<UserModel>(s => this.data.Add(s));
        return mock;
    }
}
