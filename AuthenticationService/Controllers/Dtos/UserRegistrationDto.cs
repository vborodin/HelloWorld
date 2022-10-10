namespace AuthenticationService.Controllers.Dtos;

public record UserRegistrationDto(string Username, string Password, string? Email, string? GivenName, string? Surname);