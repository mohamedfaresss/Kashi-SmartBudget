using Kashi_SmartBudget.Domain;
using Kashi_SmartBudget.Models.DTOs.Auth;
using Kashi_SmartBudget.Persistence;
using Kashi_SmartBudget.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Kashi_SmartBudget.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FullName = dto.FullName ?? "",
                Country = dto.Country ?? "",
                PhoneNumberCustom = dto.PhoneNumber ?? "",
                PreferredCurrency = dto.PreferredCurrency ?? "EGP"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken(user.Id, HttpContext.Connection.RemoteIpAddress!.ToString());

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshToken = refreshToken.Token 
            });

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromQuery] string refreshToken)
        {
            var tokenHash = Convert.ToHexString(SHA256.HashData(Convert.FromHexString(refreshToken)));

            var stored = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash && !r.Revoked);

            if (stored == null || stored.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var user = await _userManager.FindByIdAsync(stored.UserId);
            if (user == null)
                return Unauthorized("Invalid user");

            // rotate = revoke old + generate new
            stored.Revoked = true;

            var newRefresh = _tokenService.CreateRefreshToken(user.Id, HttpContext.Connection.RemoteIpAddress!.ToString());

            _context.RefreshTokens.Add(newRefresh);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = _tokenService.CreateAccessToken(user),
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                NewRefreshToken = newRefresh.TokenHash
            });
        }
    }
}
