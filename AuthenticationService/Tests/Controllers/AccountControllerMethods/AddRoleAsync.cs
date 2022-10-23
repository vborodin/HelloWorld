using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class AddRoleAsync: AccountControllerTest
{
    [Test]
    public async Task AppliesRole()
    {
        var result = await this.controller.AddRoleAsync(
            setRoleDto: new SetRoleDto(
                Username: "ExistingUser",
                Role: "NewRole"));

        Assert.True(result is OkResult);
        this.userServiceMock.Verify(s => s.AddRoleAsync("ExistingUser", "NewRole"), Times.Once);
    }

    [Test]
    public async Task RequiresExistingUser()
    {
        this.userServiceMock
            .Setup(m => m.AddRoleAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws<RoleAssignmentException>();

        var result = await this.controller.AddRoleAsync(
            setRoleDto: new SetRoleDto(
                Username: "NonExistingUser",
                Role: "NewRole"));

        Assert.True(result is BadRequestObjectResult);
    }

    [Test]
    public async Task RequiresExistingRole()
    {
        this.userServiceMock
            .Setup(m => m.AddRoleAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws<RoleExistenceException>();

        var result = await this.controller.AddRoleAsync(
            setRoleDto: new SetRoleDto(
                Username: "ExistingUser",
                Role: "NotExistingRole"));

        Assert.True(result is BadRequestObjectResult);
    }
}
