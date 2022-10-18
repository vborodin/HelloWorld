using AuthenticationService.Services.Model;
using AuthenticationService.Services.TokenGenerator;

using NUnit.Framework;

namespace AuthenticationService.Tests.TokenGeneration.UserModelTokenGeneratorMethods;

public abstract class UserModelTokenGeneratorTest
{
    protected readonly string securityKey = "1111111111111111";
    protected readonly string issuer = "issuer";
    protected ITokenGenerator<UserModel> generator = null!;
    protected UserModel data = null!;

    [SetUp]
    public void Setup()
    {
        this.generator = new UserModelTokenGenerator(this.securityKey, this.issuer);
        this.data = CreateTestData();
    }

    private UserModel CreateTestData()
    {
        return new UserModel(
            Email: "TestEmail",
            GivenName: "TestGivenName",
            Role: "TestRole",
            Surname: "TestSurname",
            Username: "TestUsername"
        );
    }
}
