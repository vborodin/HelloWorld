using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controllers.Dtos;

public record UsernameRoleDto([Required] string Username, [Required] string Role);