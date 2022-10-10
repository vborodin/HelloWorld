﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AuthenticationService.Services.Model;
using AuthenticationService.TokenGenerator;

using Microsoft.IdentityModel.Tokens;

using NUnit.Framework;

namespace AuthenticationService.Tests.TokenGeneration;

public class UserModelTokenGeneratorTest
{
    private const string securityKey = "1111111111111111";
    private const string issuer = "issuer";
    private readonly ITokenGenerator<UserModel> generator = new UserModelTokenGenerator(securityKey, issuer);

    [Test]
    public void GeneratesValidHeader()
    {
        var value = CreateTestData();
        var token = CreateDecodedToken(value, "1", 1);
        Assert.AreEqual("HS256", token.Header.Alg);
        Assert.AreEqual("JWT", token.Header.Typ);
    }

    [Test]
    public void GeneratesValidPayload()
    {
        var value = CreateTestData();
        var expirationPeriodMinutes = 1;
        var audience = "1,2";

        var token = CreateDecodedToken(value, audience, expirationPeriodMinutes);

        Assert.AreEqual(audience, token.Payload.Aud.Single());
        Assert.AreEqual(issuer, token.Payload.Iss);
        Assert.Less(DateTime.UtcNow, token.Payload.ValidTo);
        Assert.Greater(DateTime.UtcNow.AddMinutes(expirationPeriodMinutes), token.Payload.ValidTo);

        var nameIdentifier = token.Payload[ClaimTypes.NameIdentifier] as string;
        var emailAddress = token.Payload[ClaimTypes.Email] as string;
        var givenName = token.Payload[ClaimTypes.GivenName] as string;
        var surname = token.Payload[ClaimTypes.Surname] as string;
        var role = token.Payload[ClaimTypes.Role] as string;

        Assert.AreEqual(value.Username, nameIdentifier);
        Assert.AreEqual(value.Email, emailAddress);
        Assert.AreEqual(value.GivenName, givenName);
        Assert.AreEqual(value.Surname, surname);
        Assert.AreEqual(value.Role, role);
    }

    [Test]
    public void GeneratesValidSignature()
    {
        var value = CreateTestData();
        var token = this.generator.Generate(value, "1", 1);

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = "1",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
        };
        ClaimsPrincipal? claimsPrincipal = null;
        Assert.DoesNotThrow(() =>
        {
            claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);
        });
        Assert.NotNull(claimsPrincipal);
    }

    [Test]
    public void ThrowsArgumentNullExceptionForNullValue()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            this.generator.Generate(null!, "1", 1);
        });
    }

    [Test]
    public void ThrowsInvalidOperationExceptionForNullRequiredField()
    {
        var validValue = CreateTestData();
        Assert.Throws<InvalidOperationException>(() =>
        {
            this.generator.Generate(validValue with { Username = null! }, "1", 1);
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            this.generator.Generate(validValue with { Surname = null! }, "1", 1);
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            this.generator.Generate(validValue with { Role = null! }, "1", 1);
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            this.generator.Generate(validValue with { GivenName = null! }, "1", 1);
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            this.generator.Generate(validValue with { Email = null! }, "1", 1);
        });
    }

    [Test]
    public void ThrowsArgumentOutOfRangeExceptionForNonPositiveExpirationPeriod()
    {
        var value = CreateTestData();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this.generator.Generate(value, "1", 0);
        });
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this.generator.Generate(value, "1", -1);
        });
    }

    [Test]
    public void ThrowsInvalidOperationExceptionForEmptyAudience()
    {
        var value = CreateTestData();
        Assert.Throws<InvalidOperationException>(() =>
        {
            this.generator.Generate(value, "", 1);
        });
    }

    [Test]
    public void ThrowsArgumentNullExceptionForNullAudience()
    {
        var value = CreateTestData();
        Assert.Throws<ArgumentNullException>(() =>
        {
            this.generator.Generate(value, null!, 1);
        });
    }

    private UserModel CreateTestData()
    {
        return new UserModel(
            Email: "EmailValue",
            GivenName: "GivenNameValue",
            Role: "RoleValue",
            Surname: "SurnameValue",
            Username: "UsernameValue"
        );
    }

    private JwtSecurityToken CreateDecodedToken(UserModel value, string audience, int expirationPeriodMinutes)
    {
        var token = this.generator.Generate(value, audience, expirationPeriodMinutes);
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }
}
