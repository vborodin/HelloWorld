using AuthenticationService.Services.Hashing.HashingData;

using NUnit.Framework;

namespace AuthenticationService.Tests.Hashing.SaltPepperUTF8HashingDataMethods;

public class DataProperty
{
    [Test]
    public void CombinesDataSaltAndPepper()
    {
        var hashingData = new SaltPepperUTF8HashingData("1", "2", "3");
        Assert.AreEqual("123", hashingData.Data);
    }
    
    [TestCase("", "salt", "pepper")]
    [TestCase("data", "", "pepper")]
    [TestCase("data", "salt", "")]
    public void RequiresInputValuesNotToBeEmpty(string data, string salt, string pepper)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData(data, salt, pepper);
        });
        
    }

    [TestCase(null!, "salt", "pepper")]
    [TestCase("data", null!, "pepper")]
    [TestCase("data", "salt", null!)]
    public void RequiresInputValuesNotToBeNull(string data, string salt, string pepper)
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new SaltPepperUTF8HashingData(data, salt, pepper);
        });
    }
}
