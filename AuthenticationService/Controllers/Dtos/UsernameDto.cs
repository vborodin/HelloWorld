using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controllers.Dtos;

public record UsernameDto([Required] string Username);
