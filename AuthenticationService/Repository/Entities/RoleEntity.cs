namespace AuthenticationService.Repository.Entities;

public class RoleEntity
{
    public long Id { get; set; }
    public string Role { get; set; } = null!;
    public ICollection<UserEntity> Users { get; set; } = null!;
}
