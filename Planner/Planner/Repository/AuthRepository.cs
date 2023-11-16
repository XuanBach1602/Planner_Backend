using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Planner.Model;
using Planner.Repository.IRepository;
using Planner.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Planner.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IFileService fileService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthRepository(UserManager<User> userManager, IConfiguration config, SignInManager<User> signInManager, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
            this.fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> SignIn(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
            {
                return string.Empty;
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            return CreateToken(model.Email, user.Id);
        }

        public async Task<bool> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IdentityResult> SignUp(SignUpModel model)
        {
            //if (img_url == "") img_url = "https://upload.wikimedia.org/wikipedia/commons/9/99/Sample_User_Icon.png";
            var user = new User
            {
                UserName = model.Email,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public string CreateToken(string email, string userId)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:ValidIssuer"],
                audience: _config["Jwt:ValidAudience"],
                expires: DateTime.Now.AddMinutes(1000),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey,
                SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [NonAction]
        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        [NonAction]
        public void SetRefreshToken(RefreshToken newRefreshToken, User user, HttpContext context)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            context.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            _unitOfWork.Save();
        }

    }
}
