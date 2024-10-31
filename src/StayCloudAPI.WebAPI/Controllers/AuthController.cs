using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayCloudAPI.Application.DTOs.Content.AuthDto;
using StayCloudAPI.Application.Interfaces.Content.IAuth;
using StayCloudAPI.Core.Constants;
using StayCloudAPI.Core.Domain.Identity;
using StayCloudAPI.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StayCloudAPI.WebAPI.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly StayCloudAPIDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(
            StayCloudAPIDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenRepository tokenRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult> SignUp(SignUpRequestDto request)
        {
            var existingUser = await _context.Users.AnyAsync(x => x.UserName == request.UserName);

            if (existingUser) return BadRequest("Tên đăng nhập đã tồn tại!");
            if (!request.Password.Equals(request.ConfirmPassword)) return BadRequest("Mật khẩu và xác nhận mật khẩu phải giống nhau!");

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = "",
                LastName = "",
                IsActived = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.UserName,
            };

            var passwordHasher = new PasswordHasher<AppUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok("Đăng ký tài khoản thành công!");
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<ActionResult<AuthenticateResponseDto>> SignIn([FromBody] SignInRequestDto requestDto)
        {
            // Authentication
            if (requestDto == null) return BadRequest("Vui lòng nhập đầy đủ thông tin!");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == requestDto.UserName);

            if (user == null) return Unauthorized("Tên đăng nhập không tồn tại");

            var result = await _signInManager.PasswordSignInAsync(user, requestDto.Password, false, true);

            if (!result.Succeeded) return Unauthorized("Mật khẩu không đúng, vui lòng thử lại!");

            // Authorization
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, !string.IsNullOrEmpty(user.Email) ? user.Email : ""),
                new Claim(UserClaims.Id, user.Id.ToString()),
                new Claim(UserClaims.Name, !string.IsNullOrEmpty(user.UserName) ? user.UserName : ""),
                new Claim(UserClaims.FirstName, user.FirstName),
                new Claim(UserClaims.Roles, string.Join(";", roles)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var accessToken = _tokenRepository.GenerateAccessToken(claims);
            var refreshToken = _tokenRepository.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(30);
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthenticateResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
