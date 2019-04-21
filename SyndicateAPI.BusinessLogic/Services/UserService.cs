using Microsoft.IdentityModel.Tokens;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class UserService : BaseCrudService<User>, IUserService
    {
        public UserService(IRepository<User> repository) : base(repository)
        {
        }

        public bool IsEmailExist(string email)
        {
            if (GetAll().FirstOrDefault(x => x.Login == email) != null)
                return true;
            else
                return false;
        }

        public bool IsNicknameExist(string nickname)
        {
            if (GetAll().FirstOrDefault(x => x.Nickname == nickname) != null)
                return true;
            else
                return false;
        }

        public User CreateUser(string nickname, string password, Person person)
        {
            var user = new User
            {
                Login = person.Email,
                Password = password,
                Nickname = nickname,
                RegTime = DateTime.UtcNow,
                PointsCount = 0,
                Person = person,
                IsActive = false,
                ActivationCode = GetRandomNumber()
            };

            try
            {
                Create(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return user;
        }

        public string AuthorizeUser(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
                return null;

            var token = GetSecurityToken(identity);
            return token;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = GetAll().FirstOrDefault(x =>
                x.Login == username &&
                x.Password == password &&
                x.IsActive);

            if (user == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.ID.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        private string GetSecurityToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var expires = now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME));

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: expires,
                    signingCredentials: new SigningCredentials(
                        AuthOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256)
                    );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
    }
}
