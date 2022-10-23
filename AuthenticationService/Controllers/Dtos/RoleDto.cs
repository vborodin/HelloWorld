using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controllers.Dtos;

public record RoleDto([Required] string Role);
