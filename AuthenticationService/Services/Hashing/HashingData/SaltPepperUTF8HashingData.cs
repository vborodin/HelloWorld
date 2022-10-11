using System.Text;

namespace AuthenticationService.Services.Hashing.HashingData;

public class SaltPepperUTF8HashingData : IHashingData<byte[]>
{
    public SaltPepperUTF8HashingData(string data, string salt, string pepper)
    {
        data = data switch
        {
            null => throw new ArgumentNullException(nameof(data)),
            "" => throw new ArgumentException($"{nameof(data)} must not be empty"),
            _ => data
        };
        salt = salt switch
        {
            null => throw new ArgumentNullException(nameof(salt)),
            "" => throw new ArgumentException($"{nameof(salt)} must not be empty"),
            _ => salt
        };
        pepper = pepper switch
        {
            null => throw new ArgumentNullException(nameof(pepper)),
            "" => throw new ArgumentException($"{nameof(pepper)} must not be empty"),
            _ => pepper
        };

        this.Data = Encoding.UTF8.GetBytes($"{data}{salt}{pepper}");
    }

    public byte[] Data
    {
        get;
    }
}
