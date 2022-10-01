namespace AuthenticationService.Repository.Model;

public class UserModel
{
    public string? Username
    {
        get; set;
    }
    public string? Salt
    {
        get; set;
    }
    public string? PasswordHash
    {
        get; set;
    }
    public string? Email
    {
        get; set;
    }
    public string? Role
    {
        get; set;
    }
    public string? Surname
    {
        get; set;
    }
    public string? GivenName
    {
        get; set;
    }
}
