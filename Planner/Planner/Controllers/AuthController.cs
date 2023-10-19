using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly PlannerDbContext _plannerDbContext;

        public AuthController(IAuthRepository userRepository, PlannerDbContext plannerDbContext, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _plannerDbContext = plannerDbContext;
            _userManager = userManager;

        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var token = await _userRepository.SignIn(model);
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
                var userInfo = new UserInfo
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user?.Email ?? "",
                    PhoneNumber = user?.PhoneNumber ?? "",
                    Address = user?.Address ?? ""
                };
                var data = new { token, userInfo };
                return Ok(data);
            }
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _userRepository.SignUp(model);
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
            var result = await _userRepository.SignOut();
            if (result)
            {
                return Ok(new { message = "Sign out successfully" });
            }

            return BadRequest(new { message = "Sign out failed" });
        }

        public class UserInfo
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            public string Address { get; set; } = "";
        }


    }
}
