using AuthenticationService.Services.Hashing.HashCalculator;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing;

public class SHA256Base64HashCalculatorTest
{
    [Test]
    public void CalculatesHash()
    {
        var data = CreateTestData();
        var calculator = new SHA256Base64HashCalculator(1);

        var result = calculator.Calculate(data);

        // 256 bits encoded as Base64
        Assert.AreEqual(44, result.Length);
    }

    [Test]
    public void CalculatesWithSpecifiedAmountOfIterations()
    {
        var data = CreateTestData();
        var calculator1 = new SHA256Base64HashCalculator(1);
        var calculator5 = new SHA256Base64HashCalculator(5);

        var result5 = calculator5.Calculate(data);
        for (var i = 0; i < 4; i++)
        {
            var tmp = calculator1.Calculate(data);
            data = Convert.FromBase64String(tmp);
        }
        var result1 = calculator1.Calculate(data);

        Assert.AreEqual(result1, result5);
    }

    [Test]
    public void ThrowsArgumentOutOfRangeExceptionForInvalidIterations()
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

    private byte[] CreateTestData()
    {
        return new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20 };
    }
}
