using Microsoft.AspNetCore.Mvc;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPlanController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserPlanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToPlan(string userId, int planId)
        {
            await _unitOfWork.UserPlan.AddUserToPlan(planId, userId, "Member");
            try
            {
                await _unitOfWork.Save();
                return Ok("Add user to plan successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserId/{userId}")]
        public async Task<IActionResult> GetPlansByUserId(string userId)
        {
            try
            {
                var plans = await _unitOfWork.UserPlan.GetAllByFilter(x => x.UserId == userId);
                return Ok(plans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("PlanId/{planId}")]
        public async Task<IActionResult> GetUsersByPlanId(int planId)
        {
            try
            {
                var users = await _unitOfWork.UserPlan.GetAllByFilter(x => x.PlanId == planId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, string role)
        {
            try
            {
                await _unitOfWork.UserPlan.UpdateRole(id, role);
                await _unitOfWork.Save();
                return Ok("Update role successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _unitOfWork.UserPlan.RemoveUserFromPlan(id);
                await _unitOfWork.Save();
                return Ok("Remove user in plan successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
