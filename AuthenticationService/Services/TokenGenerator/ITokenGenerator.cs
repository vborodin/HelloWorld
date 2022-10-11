namespace AuthenticationService.Services.TokenGenerator;

public interface ITokenGenerator<T>
{
    string Generate(T value, string audience, int expirationPeriodMinutes);
}
