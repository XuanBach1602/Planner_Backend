using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Security.Claims;

namespace Planner.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<User> _userManager;
        private readonly PlannerDbContext _plannerDbContext;

        public AuthController(IAuthRepository userRepository, PlannerDbContext plannerDbContext, UserManager<User> userManager)
        {
            _authRepository = userRepository;
            _plannerDbContext = plannerDbContext;
            _userManager = userManager;

        }

        [HttpGet("Test")]
        public IActionResult Get()
        {
            return Ok("ok");
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var token = await _authRepository.SignIn(model);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var newRefreshToken = _authRepository.GenerateRefreshToken();
                _authRepository.SetRefreshToken(newRefreshToken, user, HttpContext);
                var userInfo = new UserInfo
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user?.PhoneNumber ?? "",
                    ImgUrl = user.ImgUrl,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth == null ? null : user.DateOfBirth.Value.ToString("yyyy-MM-dd"),
                };
                var data = new { token, userInfo };
                return Ok(data);
            }
        }

        [HttpPost("SignUp")]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> SignUp([FromForm] SignUpModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return BadRequest(new String[] { "Email is already exist" });
            }
            var result = await _authRepository.SignUp(model);
            if (result.Succeeded)
            {
                await _plannerDbContext.SaveChangesAsync();
                return Ok(new { message = "Sign Up successfully" });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(errors);
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            var result = await _authRepository.SignOut();
            if (result)
            {
                return Ok(new { message = "Sign out successfully" });
            }

            return BadRequest(new { message = "Sign out failed" });
        }

        [Authorize]
        [HttpPost("GetRefreshToken")]
        public ActionResult RefreshToken()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var refreshToken = Request.Cookies["refreshToken"];

            if (claim != null)
            {

                string userId = claim.Value;
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                if (!user.RefreshToken.Equals(refreshToken))
                {
                    return Unauthorized("Invalid Refresh Token.");
                }
                else if (user.TokenExpires < DateTime.Now)
                {
                    return Unauthorized("Token expired.");
                }
                string token = _authRepository.CreateToken(user.Email, user.Id);
                var newRefreshToken = _authRepository.GenerateRefreshToken();
                _authRepository.SetRefreshToken(newRefreshToken, user, HttpContext);

                return Ok(token);
            }
            else
            {
                return NotFound("User not found");
            }





        }

        public class UserInfo
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            public string ImgUrl { get; set; } = "";

            public string Gender { get; set; } = "";
            public string DateOfBirth { get; set; } = "";
        }


    }
}
