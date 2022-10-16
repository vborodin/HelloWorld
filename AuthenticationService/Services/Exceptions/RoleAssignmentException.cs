namespace AuthenticationService.Services.Exceptions;

public class RoleAssignmentException: Exception
{
    public RoleAssignmentException(): base() { }

    public RoleAssignmentException(string message): base(message) { }

    public RoleAssignmentException(string message, Exception inner): base(message, inner) { }
}
