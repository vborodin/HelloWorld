using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.RoleControllerMethods;

public class CreateRoleAsync : RoleControllerTest
{
    [Test]
    public async Task CreatesRole()
    {
        var result = await this.roleController.CreateRoleAsync(
            roleDto: new RoleDto("NewRole"));

        Assert.True(result is OkResult);
        this.roleServiceMock.Verify(m => m.CreateRoleAsync("NewRole"), Times.Once);
    }

    [Test]
    public async Task RequiresRoleToBeUnique()
    {
        this.roleServiceMock
            .Setup(m => m.CreateRoleAsync(It.IsAny<string>()))
            .Throws<RoleExistenceException>();

        var result = await this.roleController.CreateRoleAsync(
            roleDto: new RoleDto("ExistingRole"));

        Assert.True(result is BadRequestObjectResult);
        this.roleServiceMock.Verify(m => m.CreateRoleAsync("ExistingRole"), Times.Once);
    }
}
