using Kashi.Domain;
using Kashi_SmartBudget.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Kashi_SmartBudget.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }


        public string CreateAccessToken(ApplicationUser User)
        {

            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));




            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, User.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, User.Email));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: jwt["Issuer"],
               audience: jwt["Audience"],
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["AccessTokenMinutes"]!)),
               signingCredentials: creds
           );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public RefreshToken CreateRefreshToken(string userId, string createdByIp)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            // raw token (HEX) — ده اللي هيروح للعميل
            var rawToken = Convert.ToHexString(randomBytes);

            // hashed token — ده اللي يتخزن DB
            var tokenHash = Convert.ToHexString(SHA256.HashData(randomBytes));

            return new RefreshToken
            {
                UserId = userId,
                Token = rawToken,      
                TokenHash = tokenHash,  
                ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:RefreshTokenDays"]!)),
                CreatedByIp = createdByIp
            };
        }

    }
}

