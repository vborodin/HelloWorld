using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controllers.Dtos;

public record UsernamePasswordDto([Required] string Username, [Required] string Password);
