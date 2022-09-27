using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AuthenticationService.Models;

using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.TokenGenerator
{
    public class UserModelTokenGenerator : ITokenGenerator<UserModel>
    {
        private readonly SigningCredentials credentials;
        private readonly string issuer;
        private readonly string audience;
        private readonly int expirationPeriodMinutes;

        public UserModelTokenGenerator(string key, string issuer, string audience, int expirationPeriodMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            this.credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            this.issuer = issuer;
            this.audience = audience;
            this.expirationPeriodMinutes = expirationPeriodMinutes;
        }

        public string Generate(UserModel value)
        {
            VerifyUserModel(value);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, value.Username!),
                new Claim(ClaimTypes.Email, value.Email!),
                new Claim(ClaimTypes.GivenName, value.GivenName!),
                new Claim(ClaimTypes.Surname, value.Surname!),
                new Claim(ClaimTypes.Role, value.Role!)
            };
            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims, 
                expires: DateTime.Now.AddMinutes(this.expirationPeriodMinutes),
                signingCredentials: this.credentials);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        private void VerifyUserModel(UserModel value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{nameof(UserModel)} value cannot be null");
            }
            if (value.Username == null || 
                value.Email == null || 
                value.GivenName == null || 
                value.Surname == null || 
                value.Role == null)
            {
                throw new InvalidOperationException($"Required field of {nameof(UserModel)} is null");
            }
        }
    }
}
