using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using NUnit.Framework;

namespace AuthenticationService.Tests.TokenGeneration.UserModelTokenGeneratorMethods;

public class Generate: UserModelTokenGeneratorTest
{
    [Test]
    public void GeneratesValidHeader()
    {
        var token = Decode(
            this.generator.Generate(this.data, "1", 1));
        
        Assert.AreEqual("HS256", token.Header.Alg);
        Assert.AreEqual("JWT", token.Header.Typ);
    }

    [Test]
    public void GeneratesValidPayload()
    {
        var expirationPeriodMinutes = 1;
        var audience = "1,2";

        var token = Decode(
            this.generator.Generate(this.data, "1,2", expirationPeriodMinutes));

        var nameIdentifier = token.Payload[ClaimTypes.NameIdentifier] as string;
        var emailAddress = token.Payload[ClaimTypes.Email] as string;
        var givenName = token.Payload[ClaimTypes.GivenName] as string;
        var surname = token.Payload[ClaimTypes.Surname] as string;
        var role = token.Payload[ClaimTypes.Role] as string;
        Assert.AreEqual(audience, token.Payload.Aud.Single());
        Assert.AreEqual(this.issuer, token.Payload.Iss);
        Assert.Less(DateTime.UtcNow, token.Payload.ValidTo);
        Assert.Greater(DateTime.UtcNow.AddMinutes(expirationPeriodMinutes), token.Payload.ValidTo);
        Assert.AreEqual(this.data.Username, nameIdentifier);
        Assert.AreEqual(this.data.Email, emailAddress);
        Assert.AreEqual(this.data.GivenName, givenName);
        Assert.AreEqual(this.data.Surname, surname);
        Assert.AreEqual(this.data.Role, role);
    }

    [Test]
    public void GeneratesValidSignature()
    {
        var token = this.generator.Generate(this.data, "1", 1);

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = "1",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.securityKey))
        };
        ClaimsPrincipal? claimsPrincipal = null;
        Assert.DoesNotThrow(() =>
        {
            claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);
        });
        Assert.NotNull(claimsPrincipal);
    }

    private JwtSecurityToken Decode(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }
}
