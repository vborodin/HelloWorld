using AuthenticationService.Hashing.HashingData;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing;

public class SaltPepperUTF8HashingDataTest
{
    [Test]
    public void CombinesDataSaltAndPepper()
    {
        var hashingData = new SaltPepperUTF8HashingData("1", "2", "3");
        Assert.AreEqual("123", hashingData.Data);
    }

    [Test]
    public void ThrowsArgumentExceptionForInvalidData()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData("", "2", "3");
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData(null!, "2", "3");
        });
    }

    [Test]
    public void ThrowsArgumentExceptionForInvalidSalt()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData("1", "", "3");
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData("1", null!, "3");
        });
    }

    [Test]
    public void ThrowsArgumentExceptionForInvalidPepper()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData("1", "2", "");
        });
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData("1", "2", null!);
        });
    }
}
