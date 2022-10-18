using AuthenticationService.Services.Hashing.HashCalculator;
using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing.SHA256Base64HashCalculatorMethods;

public class Calculate: SHA256Base64HashCalculatorTest
{
    [Test]
    public void Calculates256BitsHash()
    {
        var calculator = new SHA256Base64HashCalculator(1);

        var result = calculator.Calculate(this.testData);

        // 256 bits encoded as Base64
        Assert.AreEqual(44, result.Length);
    }

    [Test]
    public void CalculatesWithSpecifiedAmountOfIterations()
    {
        var calculator1 = new SHA256Base64HashCalculator(1);
        var calculator5 = new SHA256Base64HashCalculator(5);

        var result5 = calculator5.Calculate(this.testData);
        for (var i = 0; i < 4; i++)
        {
            var tmp = calculator1.Calculate(this.testData);
            this.testData = Convert.FromBase64String(tmp);
        }
        var result1 = calculator1.Calculate(this.testData);

        Assert.AreEqual(result1, result5);
    }
}
