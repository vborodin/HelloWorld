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
            this.generator.Generate(this.userModel, "1", 1));
        
        Assert.AreEqual("HS256", token.Header.Alg);
        Assert.AreEqual("JWT", token.Header.Typ);
    }

    [Test]
    public void GeneratesValidPayload()
    {
        var expirationPeriodMinutes = 1;
        var audience = "1";
        var userModel = this.userModel with
        {
            Roles = new List<string>()
            {
                "User",
                "Administrator"
            }
        };

        var token = Decode(
            this.generator.Generate(userModel, audience, expirationPeriodMinutes));

        var nameIdentifier = token.Claims
            .Single(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Value;
        var roles = token.Claims
            .Where(claim => claim.Type == ClaimTypes.Role)
            .Select(x => x.Value);
        
        Assert.AreEqual(audience, token.Payload.Aud.Single());
        Assert.AreEqual(this.issuer, token.Payload.Iss);
        Assert.Less(DateTime.UtcNow, token.Payload.ValidTo);
        Assert.Greater(DateTime.UtcNow.AddMinutes(expirationPeriodMinutes), token.Payload.ValidTo);
        Assert.AreEqual(userModel.Username, nameIdentifier);
        Assert.AreEqual(userModel.Roles, roles);
    }

    [Test]
    public void GeneratesValidSignature()
    {
        var token = this.generator.Generate(this.userModel, "1", 1);

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
