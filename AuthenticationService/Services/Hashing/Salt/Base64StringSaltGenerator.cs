using System.Security.Cryptography;

namespace AuthenticationService.Services.Hashing.Salt;

public class Base64StringSaltGenerator : ISaltGenerator<string>, IDisposable
{
    private readonly RandomNumberGenerator rng;
    private readonly int size;

    public Base64StringSaltGenerator(int size)
    {
        if (size < 4 || size % 4 != 0)
        {
            throw new ArgumentException($"{nameof(size)} must be positive multiple of 4");
        }
        this.size = size;
        this.rng = RandomNumberGenerator.Create();
    }

    public void Dispose()
    {
        this.rng.Dispose();
    }

    public string Generate()
    {
        var byteSalt = new byte[this.size / 4 * 3]; // 3 bytes are encoded by 4 characters
        this.rng.GetBytes(byteSalt);
        var salt = Convert.ToBase64String(byteSalt);
        return salt;
    }
}
