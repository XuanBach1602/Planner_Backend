using Microsoft.AspNetCore.Mvc;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            if (users == null)
            {
                return NotFound();
            }
            var usersModel = users.Select(user => new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                //Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImgUrl

            });

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

            return Ok(new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                //Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImgUrl
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserModel userModel)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("The user's data is invalid");
            }
            user.PhoneNumber = userModel.PhoneNumber;
            //user.Address = userModel.Address;
            user.Email = userModel.Email;
            user.Name = userModel.Name;
            user.ImgUrl = userModel.ImageUrl;
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
        public async Task<IActionResult> GetByPlanID(int? planID)
        {
            if (planID == null)
            {
                return BadRequest("PlanID is required");
            }

            var user = await _unitOfWork.User.GetUsersByPlanID(planID.Value);
            return Ok(user);

        }

        public class UserModel
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
        }

    }
}
