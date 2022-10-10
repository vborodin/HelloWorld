using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Repository.Entities;

public class UserEntity
{
    public long Id { get; set; }
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Salt { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;
    public string? Email { get; set; }
    [Required]
    public string Role { get; set; } = null!;
    public string? Surname { get; set; }
    public string? GivenName { get; set; }
}
