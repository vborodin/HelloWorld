using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Repository.Model;

public class UserModel
{
    public long Id { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Salt { get; set; }
    [Required]
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }
    [Required]
    public string? Role { get; set; }
    public string? Surname { get; set; }
    public string? GivenName { get; set; }
}
