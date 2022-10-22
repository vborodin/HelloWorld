using AuthenticationService.Services.Hashing.Salt;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing.Base64StringSaltGeneratorMethods;

[Category("Hashing")]
public class Constructor
{
    [Test]
    public void RequiresSizeToBeMultipleOf4()
    {
        for (var i = 0; i < 10; i++)
        {
            if (i < 4 || i % 4 != 0)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new Base64StringSaltGenerator(i);
                });
            }
        }
    }
}
