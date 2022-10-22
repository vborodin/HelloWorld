using AuthenticationService.Services.Hashing.Salt;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing.Base64StringSaltGeneratorMethods;

[Category("Hashing")]
public class Generate
{
    [Test]
    public void GeneratesSaltOfSpecifiedSize()
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
}