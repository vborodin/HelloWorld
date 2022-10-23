using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.RoleControllerMethods;

public class DeleteRoleAsync : RoleControllerTest
{
    [Test]
    public async Task DeletesRole()
    {
        var result = await this.roleController.DeleteRoleAsync(
            roleDto: new RoleDto("ExistingRole"));

        Assert.True(result is OkResult);
        this.roleServiceMock.Verify(m => m.DeleteRoleAsync("ExistingRole"), Times.Once);
    }

    [Test]
    public async Task RequiresRoleToExist()
    {
        this.roleServiceMock
            .Setup(m => m.DeleteRoleAsync(It.IsAny<string>()))
            .Throws<RoleExistenceException>();

        var result = await this.roleController.DeleteRoleAsync(
            roleDto: new RoleDto("NonExistingRole"));

        Assert.True(result is BadRequestObjectResult);
        this.roleServiceMock.Verify(m => m.DeleteRoleAsync("NonExistingRole"), Times.Once);
    }
}
