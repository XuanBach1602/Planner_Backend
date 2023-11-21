using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;
using Planner.Services;

namespace Planner.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            if (users == null)
            {
                return NotFound();
            }
            var usersModel = users.Select(user => ConvertToUserModel(user)).ToList();

            return Ok(usersModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("Can not find the user");
            }
            var userInfo = ConvertToUserModel(user);
            return Ok(userInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateModel update)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("The user's data is invalid");
            }
            var ImgUrl = await _fileService.UploadFile(update.File);
            _fileService.DeleteFile(user.ImgUrl);
            user.ImgUrl = ImgUrl;
            user.PhoneNumber = update.PhoneNumber;
            user.Email = update.Email;
            user.Name = update.Name;
            user.Gender = update.Gender;
            user.DateOfBirth = update.DateOfBirth;

            _unitOfWork.User.Update(user);
            try
            {
                await _unitOfWork.Save();
                return Ok("Update successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetByPlanID/{planID}")]
        public async Task<IActionResult> GetByPlanID(int planID)
        {
            var user = await _unitOfWork.User.GetUsersByPlanID(planID);
            return Ok(user);

        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return StatusCode(204, new { message = "Not found" });
            }
            var userModel = ConvertToUserModel(user);
            return Ok(userModel);
        }



        public class UserModel
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string ImgUrl { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
            public string DateOfBirth { get; set; } = string.Empty;
        }

        public class UpdateModel
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
            public DateTime DateOfBirth { get; set; }
            public IFormFile? File { get; set; }
        }

        [NonAction]
        public UserModel ConvertToUserModel(User user)
        {
            return new UserModel
            {

                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ImgUrl = user.ImgUrl,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth != null ? user.DateOfBirth.Value.ToString("yyyy-MM-dd") : "",
            };

        }

    }
}
