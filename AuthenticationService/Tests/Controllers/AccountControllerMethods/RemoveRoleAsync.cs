using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class RemoveRoleAsync : AccountControllerTest
{
    [Test]
    public async Task RemovesRole()
    {
        var usernameRoleDto = new UsernameRoleDto(Username: "ExistingUsername", Role: "ExistingRole");

        var result = await this.controller.RemoveRoleAsync(usernameRoleDto);

        Assert.True(result is OkResult);
        this.userServiceMock.Verify(m => m.RemoveRoleAsync("ExistingUsername", "ExistingRole"), Times.Once);
    }

    [Test]
    public async Task RequiresExistingUserAndAssignedRole()
    {
        var usernameRoleDto = new UsernameRoleDto(Username: "NonExistingUsername", Role: "NotAssignedRole");
        this.userServiceMock
            .Setup(m => m.RemoveRoleAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws<RoleAssignmentException>();

        var result = await this.controller.RemoveRoleAsync(usernameRoleDto);

        Assert.True(result is BadRequestObjectResult);
    }
}
