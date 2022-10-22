using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing.SHA256Base64HashCalculatorMethods;

[Category("Hashing")]
public abstract class SHA256Base64HashCalculatorTest
{
    protected byte[] testData = null!;

    [SetUp]
    public void Setup()
    {
        this.testData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20 };
    }
}
