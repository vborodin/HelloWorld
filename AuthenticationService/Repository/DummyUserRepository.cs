﻿using AuthenticationService.Models;
using AuthenticationService.Repository.Filter;

namespace AuthenticationService.Repository
{
    public class DummyUserRepository : IUserRepository
    {
        public IEnumerable<UserModel> Get(IFilter<UserModel> filter)
        {
            return new List<UserModel>()
            { 
                new UserModel() { Email = "dummy", GivenName = "dummy", PasswordHash = "dummy", Username = "dummy", Role = "Administrator", Surname = "dummy" }
            };
        }
    }
}
