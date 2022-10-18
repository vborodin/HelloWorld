using AuthenticationService.Services.Hashing.HashCalculator;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing.SHA256Base64HashCalculatorMethods;

public class Constructor: SHA256Base64HashCalculatorTest
{
    [Test]
    public void RequiresPositiveIterations()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new SHA256Base64HashCalculator(0);
        });
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new SHA256Base64HashCalculator(-1);
        });
    }
}
