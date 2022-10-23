using AuthenticationService.Controllers;
using AuthenticationService.Services;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.RoleControllerMethods;

[Category("Controllers")]
public abstract class RoleControllerTest
{
    protected RoleController roleController = null!;
    protected Mock<IRoleService> roleServiceMock = null!;

    [SetUp]
    public void Setup()
    {
        this.roleServiceMock = new Mock<IRoleService>();
        this.roleController = new RoleController(this.roleServiceMock.Object);
    }
}
