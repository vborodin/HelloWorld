using AuthenticationService.Repository;
using AuthenticationService.Repository.Entities;
using AuthenticationService.Repository.Filter;
using AuthenticationService.Services.Exceptions;
using AuthenticationService.Services.Hashing.HashCalculator;
using AuthenticationService.Services.Hashing.HashingData;
using AuthenticationService.Services.Hashing.Salt;
using AuthenticationService.Services.Model;

namespace AuthenticationService.Services;

public class UserService : IUserService
{
    private readonly IRepository<UserEntity> userRepository;
    private readonly IRepository<RoleEntity> roleRepository;
    private readonly IHashCalculator<byte[], string> hashCalculator;
    private readonly ISaltGenerator<string> saltGenerator;
    private readonly string pepper;

    public UserService(
        IRepository<UserEntity> userRepository,
        IRepository<RoleEntity> roleRepository,
        IHashCalculator<byte[], string> hashCalculator,
        ISaltGenerator<string> saltGenerator,
        string pepper)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.hashCalculator = hashCalculator;
        this.saltGenerator = saltGenerator;
        this.pepper = pepper;
    }

    public async Task CreateUserAsync(string username, string password)
    {
        var defaultRole = "User";
        var filter = new RoleFilter(defaultRole);
        var roleEntity = await this.roleRepository.GetAsync(filter).SingleOrDefaultAsync();
        roleEntity = roleEntity ?? throw new RoleAssignmentException($"Default {nameof(RoleEntity)} \"{defaultRole}\" does not exist in the database");

        var salt = this.saltGenerator.Generate();
        var hashingData = new SaltPepperUTF8HashingData(password, salt, this.pepper);
        var hash = this.hashCalculator.Calculate(hashingData.Data);

        var entity = new UserEntity()
        {
            Username = username,
            PasswordHash = hash,
            Salt = salt,
            Roles = new List<RoleEntity> { roleEntity }
        };

        try
        {
            await this.userRepository.CreateAsync(entity);
        }
        catch (InvalidOperationException e)
        {
            throw new RegistrationException(e.Message, e);
        }
    }

    public async Task<UserModel?> GetUserAsync(string username, string password)
    {
        var filter = new UsernameFilter(username);
        var userEntity = await this.userRepository.GetAsync(filter).SingleOrDefaultAsync();
        if (userEntity != null && IsPasswordValid(password, userEntity.PasswordHash, userEntity.Salt))
        {
            return new UserModel(
                Username: userEntity.Username,
                Roles: userEntity.Roles.Select(r => r.Role));
        }
        return null;
    }

    public async Task AddRoleAsync(string username, string role)
    {
        var userFilter = new UsernameFilter(username);
        var userEntity = await this.userRepository.GetAsync(userFilter).SingleOrDefaultAsync();
        userEntity = userEntity ?? throw new RoleAssignmentException($"{nameof(UserEntity)} \"{username}\" does not exist");
        
        var roleFilter = new RoleFilter(role);
        var roleEntity = await this.roleRepository.GetAsync(roleFilter).SingleOrDefaultAsync();
        roleEntity = roleEntity ?? throw new RoleAssignmentException($"{nameof(RoleEntity)} \"{role}\" does not exist");
        userEntity.Roles.Add(roleEntity);
        await this.userRepository.UpdateAsync(userEntity);
    }

    public async Task RemoveRoleAsync(string username, string role)
    {
        var filter = new UsernameFilter(username);
        var userEntity = await this.userRepository.GetAsync(filter).SingleOrDefaultAsync();
        userEntity = userEntity ?? throw new RoleAssignmentException($"{nameof(UserEntity.Username)} \"{username}\" does not exist");
        
        var roleEntity = userEntity.Roles.SingleOrDefault(x => x.Role == role);
        roleEntity = roleEntity ?? throw new RoleAssignmentException($"{nameof(RoleEntity)} \"{role}\" is not assigned to {nameof(UserEntity)} \"{username}\"");

        userEntity.Roles.Remove(roleEntity);
        await this.userRepository.UpdateAsync(userEntity);
    }

    private bool IsPasswordValid(string password, string passwordHash, string salt)
    {
        var hashingData = new SaltPepperUTF8HashingData(password, salt, this.pepper);
        var hash = this.hashCalculator.Calculate(hashingData.Data);
        return hash == passwordHash;
    }
}
