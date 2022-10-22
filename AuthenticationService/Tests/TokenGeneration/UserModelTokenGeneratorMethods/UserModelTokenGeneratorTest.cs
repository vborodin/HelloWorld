using AuthenticationService.Services.Model;
using AuthenticationService.Services.TokenGenerator;

using NUnit.Framework;

namespace AuthenticationService.Tests.TokenGeneration.UserModelTokenGeneratorMethods;

[Category("JWTGeneration")]
public abstract class UserModelTokenGeneratorTest
{
    protected readonly string securityKey = "1111111111111111";
    protected readonly string issuer = "issuer";
    protected ITokenGenerator<UserModel> generator = null!;
    protected UserModel userModel = null!;

    [SetUp]
    public void Setup()
    {
        this.generator = new UserModelTokenGenerator(this.securityKey, this.issuer);
        this.userModel = CreateTestData();
    }

    private UserModel CreateTestData()
    {
        return new UserModel(
            Username: "TestUsername",
            Roles: new List<string>()
        );
    }
}
