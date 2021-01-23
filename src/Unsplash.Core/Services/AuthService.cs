using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unsplash.Core.ApiModels;
using Unsplash.Core.DataAccess;
using Unsplash.Core.Models;
using Unsplash.Core.Util;

namespace Unsplash.Core.Services.Interfaces
{
  public class AuthService : IAuthService
    {
        private readonly IRepository<User> repository;
        private readonly ILogger<AuthService> log;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IRepository<User> repository, ILogger<AuthService> log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.log = log;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TokenModel> LogUserIn(LoginModel model)
        {
            if (model == null) return null;
            User user = new User();
            var userContext = _httpContextAccessor.HttpContext.User.Identity.Name;
            //var userIdentity = (ClaimsIdentity)userContext.Identity;
            //var claim = userIdentity.Claims.ToList();
            //var roleClaimType = userIdentity.RoleClaimType;
            //var roles = claim.Where(c => c.Type == ClaimTypes.Role).Select(d => d.Value).ToList();
            log.LogInformation($"=>> {userContext}");
            try
            {

                user = await repository.FirstOrDefault(u => u.Username == model.Username);
                if (user == null) return null;
            }
            catch (Exception e)
            {
                log.LogError($"{e.Message}");
            }


            var verifyPwd = AuthUtil.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt);
            if (!verifyPwd) return null;

            var claims = new ClaimsIdentity(new[] { new Claim("id", $"{user.ID}"), new Claim(ClaimTypes.Role, user.Role), new Claim(ClaimTypes.Name, user.Username) });
            var jwtSecret = configuration["JwtSettings:Secret"];
            var token = AuthUtil.GenerateJwtToken(jwtSecret, claims);
            claims.AddClaim(new Claim("token", token));

            var refreshToken = AuthUtil.GenerateRefreshToken();

            // Save tokens to DB
            user.AuthToken = token;
            user.RefreshToken = refreshToken;

            await repository.Update(user);
            return new TokenModel
            {
                Token = token,
                RefreshToken = refreshToken,
                Email = user.Email,
                UserID = user.ID,
                Role = user.Role,
                Username = user.Username
            };
            //throw new NotImplementedException("h");
        }

        public Task<(UserModel user, string message)> LogUserOut(SignUpModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<(UserModel user, string message)> RegisterUser(SignUpModel model)
        {
            var userExists = await repository.FirstOrDefault(r => r.Username == model.Username);

            if (userExists == null)
            {
                AuthUtil.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var userDetails = new User
                {
                    CreatedAt = DateTime.Now,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Username = model.Username,
                    Email = model.Email,
                    Role = UserRoles.AppUser
                };

                var newuser = await repository.Insert(userDetails);
                var returnView = new UserModel
                {
                    Username = newuser.Username,
                    Email = newuser.Email,
                    Id=newuser.ID
                };

                return (user: returnView, message: "User created successfully.");
            }
            return (user: null, message: $"User {model.Username} exists already!");
        }

    }
}