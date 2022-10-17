using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class SetRole: AccountControllerTest
{
    [Test]
    public async Task SetsRole()
    {
        var result = await this.controller.SetRole(
            setRoleDto: new SetRoleDto(
                Username: "ExistingUser",
                Role: "NewRole"));

        Assert.True(result is OkResult);
        this.userServiceMock.Verify(s => s.SetRoleAsync("ExistingUser", "NewRole"), Times.Once);
    }

    [Test]
    public async Task RequiresExistingUser()
    {
        this.userServiceMock
            .Setup(m => m.SetRoleAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws<RoleAssignmentException>();

        var result = await this.controller.SetRole(new SetRoleDto(
            Username: "InvalidUser",
            Role: "NewRole"));

        Assert.True(result is BadRequestObjectResult);
    }
}
