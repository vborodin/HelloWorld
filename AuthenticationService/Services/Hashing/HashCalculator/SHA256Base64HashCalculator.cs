using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Services.Hashing.HashCalculator;

public class SHA256Base64HashCalculator : IHashCalculator<byte[], string>, IDisposable
{
    private readonly int iterations;
    private readonly SHA256 sha256;

    public SHA256Base64HashCalculator(int iterations)
    {
        this.iterations = iterations switch
        {
            < 1 => throw new ArgumentOutOfRangeException(nameof(iterations)),
            _ => iterations
        };
        this.sha256 = SHA256.Create();
    }

    public string Calculate(byte[] data)
    {
        for (var i = 0; i < this.iterations; i++)
        {
            data = this.sha256.ComputeHash(data);
        }
        var hash = Convert.ToBase64String(data);
        return hash;
    }

    public void Dispose()
    {
        this.sha256.Dispose();
    }
}
