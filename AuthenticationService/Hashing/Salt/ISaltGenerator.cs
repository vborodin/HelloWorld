namespace AuthenticationService.Hashing.Salt;

public interface ISaltGenerator<T>
{
    T Generate();
}
