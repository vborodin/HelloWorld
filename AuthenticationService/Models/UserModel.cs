namespace AuthenticationService.Models
{
    public class UserModel
    {
        public string? Username { get; set; }
        // TODO: Store password securely
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Surname { get; set; }
        public string? GivenName { get; set; }
    }
}
