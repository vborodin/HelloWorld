using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class DeleteAsync : AccountControllerTest
{
    [Test]
    public async Task DeletesUser()
    {
        var result = await this.controller.DeleteAsync(new UsernameDto("ExistingUser"));

        this.userServiceMock.Verify(m => m.DeleteUserAsync("ExistingUser"), Times.Once);
        Assert.True(result is OkResult);
    }

    [Test]
    public async Task RequiresExistingUsername()
    {
        this.userServiceMock
            .Setup(m => m.DeleteUserAsync(It.IsAny<string>()))
            .Throws<UserExistenceException>();

        var result = await this.controller.DeleteAsync(new UsernameDto("NonExistingUser"));

        Assert.True(result is BadRequestObjectResult);
    }
}
