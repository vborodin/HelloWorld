using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository.Entities;

[Index(nameof(Username), IsUnique = true)]
public class UserEntity
{
    public long Id { get; set; }
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Salt { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;
    [Required]
    public ICollection<RoleEntity> Roles { get; set; } = null!;
}
