using AuthenticationService.Hashing.Salt;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing;

public class StringSaltGeneratorTest
{
    [Test]
    public void GeneratesBase64EncodedSalt()
    {
        var generator = new Base64StringSaltGenerator(16);
        var salt = generator.Generate();
        Assert.AreEqual(16, salt.Length);
    }

    [Test]
    public void GeneratesSaltRandomly()
    {
        var generator = new Base64StringSaltGenerator(16);
        var salt1 = generator.Generate();
        var salt2 = generator.Generate();
        var salt3 = generator.Generate();

        Assert.AreNotEqual(salt1, salt2);
        Assert.AreNotEqual(salt1, salt3);
        Assert.AreNotEqual(salt2, salt3);
    }

    [Test]
    public void ThrowsArgumentExceptionIfSizeIsNotMultipleOfFour()
    {
        for (var i = 0; i < 10; i++)
        {
            if (i < 4 || i % 4 != 0)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Base64StringSaltGenerator(3);
                });
            }
        }
    }
}
