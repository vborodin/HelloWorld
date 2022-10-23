namespace AuthenticationService.Services.Exceptions;

public class RoleExistenceException : Exception
{
    public RoleExistenceException() : base() { }

    public RoleExistenceException(string message) : base(message) { }

    public RoleExistenceException(string message, Exception inner) : base(message, inner) { }
}
