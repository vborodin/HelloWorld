namespace AuthenticationService.Services.Exceptions;

public class UserExistenceException : Exception
{
    public UserExistenceException() : base() { }

    public UserExistenceException(string message) : base(message) { }

    public UserExistenceException(string message, Exception inner) : base(message, inner) { }
}
