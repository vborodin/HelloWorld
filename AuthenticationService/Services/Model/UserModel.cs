namespace AuthenticationService.Services.Model;

public record UserModel(string Username, IEnumerable<string> Roles);
