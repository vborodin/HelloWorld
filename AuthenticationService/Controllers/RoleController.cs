using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;

    public RoleController(IRoleService roleService)
    {
        this.roleService = roleService;
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> CreateRoleAsync([FromBody] RoleDto roleDto)
    {
        try
        {
            await this.roleService.CreateRoleAsync(roleDto.Role);
        }
        catch (RoleExistenceException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> DeleteRoleAsync([FromBody] RoleDto roleDto)
    {
        try
        {
            await this.roleService.DeleteRoleAsync(roleDto.Role);
        }
        catch (RoleExistenceException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }
}
