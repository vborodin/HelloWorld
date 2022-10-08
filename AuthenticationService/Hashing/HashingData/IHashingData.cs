namespace AuthenticationService.Hashing.HashingData;

public interface IHashingData<T>
{
    T Data
    {
        get;
    }
}
