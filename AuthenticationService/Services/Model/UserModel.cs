namespace AuthenticationService.Services.Model;

public record UserModel(string Username, string? Email, string Role, string? Surname, string? GivenName);
