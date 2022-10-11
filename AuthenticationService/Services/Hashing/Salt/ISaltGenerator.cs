namespace AuthenticationService.Services.Hashing.Salt;

public interface ISaltGenerator<T>
{
    T Generate();
}
