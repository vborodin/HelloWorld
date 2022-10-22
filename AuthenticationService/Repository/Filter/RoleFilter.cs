using AuthenticationService.Repository.Entities;

namespace AuthenticationService.Repository.Filter;

public class RoleFilter : IFilter<RoleEntity>
{
    private readonly string role;

    public RoleFilter(string role)
    {
        this.role = role;
    }

    public IQueryable<RoleEntity> Apply(IQueryable<RoleEntity> source)
    {
        return source.Where(x => x.Role == this.role);
    }
}
