using AuthenticationService.Models;
using AuthenticationService.Repository.Filter;

using NUnit.Framework;

namespace AuthenticationService.Tests
{
    public class UsernamePasswordFilterTest
    {
        private readonly IEnumerable<UserModel> source = CreateTestData();

        [Test]
        public void ReturnsCorrectUserModel()
        {
            var filter = new UsernamePasswordFilter("1", "1");
            var result = filter.Filter(source);
            var match = result.Single();
            Assert.True(match.Username == "1" && match.Password == "1");
        }

        [Test]
        public void ReturnsAllUserModels()
        {
            var filter = new UsernamePasswordFilter("2", "2");
            var result = filter.Filter(source);
            Assert.True(result.Count() == 2);
        }

        [Test]
        public void ReturnsNothingForIncorrectLogin()
        {
            var filter = new UsernamePasswordFilter("nonexistent", "1");
            var result = filter.Filter(source);
            Assert.IsEmpty(result);
        }

        [Test]
        public void ReturnsNothingForIncorrectPassword()
        {
            var filter = new UsernamePasswordFilter("1", "nonexistent");
            var result = filter.Filter(source);
            Assert.IsEmpty(result);
        }

        [Test]
        public void UsernameIsCaseInsensitive()
        {
            var filter = new UsernamePasswordFilter("john", "2");
            var result = filter.Filter(source);
            Assert.True(result.Count() == 1);
        }

        private static IEnumerable<UserModel> CreateTestData()
        {
            return new List<UserModel> {
                new () { Username = "1", Password = "1" },
                new () { Username = "1", Password = "2" },
                new () { Username = "2", Password = "2" },
                new () { Username = "2", Password = "2" },
                new () { Username = "John", Password = "2" },
                new () { Username = null, Password = "1" },
                new () { Username = "1", Password = null },
                new () { Username = null, Password = null }
            };
        }
    }
}
