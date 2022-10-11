namespace AuthenticationService.Services.Hashing.HashingData;

public interface IHashingData<T>
{
    T Data
    {
        get;
    }
}
