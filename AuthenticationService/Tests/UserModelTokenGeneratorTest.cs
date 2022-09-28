using AuthenticationService.Models;
using AuthenticationService.TokenGenerator;

using NUnit.Framework;

namespace AuthenticationService.Tests
{
    public class UserModelTokenGeneratorTest
    {
        private readonly ITokenGenerator<UserModel> generator = new UserModelTokenGenerator("1111111111111111", "2", "3", 1);

        [Test]
        public void ThrowsArgumentNullExceptionForNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => 
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                generator.Generate(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            });
        }

        [Test]
        public void ThrowsInvalidOperationExceptionForNullRequiredField()
        {
            var value = CreateTestData();
            value.Username = null;
            Assert.Throws<InvalidOperationException>(() =>
            {
                generator.Generate(value);
            });
            value.Username = "UsernameValue";

            value.Surname = null;
            Assert.Throws<InvalidOperationException>(() =>
            {
                generator.Generate(value);
            });
            value.Surname = "SurnameValue";

            value.Role = null;
            Assert.Throws<InvalidOperationException>(() =>
            {
                generator.Generate(value);
            });
            value.Role = "RoleValue";

            value.GivenName = null;
            Assert.Throws<InvalidOperationException>(() =>
            {
                generator.Generate(value);
            });
            value.GivenName = "GivenNameValue";

            value.Email = null;
            Assert.Throws<InvalidOperationException>(() =>
            {
                generator.Generate(value);
            });
        }

        [Test]
        public void GeneratesStringTokenForValidUserModel()
        {
            var value = CreateTestData();
            var result = generator.Generate(value);
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
        }

        private UserModel CreateTestData()
        {
            return new UserModel()
            {
                Email = "EmailValue",
                GivenName = "GivenNameValue",
                Password = "PasswordValue",
                Role = "RoleValue",
                Surname = "SurnameValue",
                Username = "UsernameValue"
            };
        }
    }
}
