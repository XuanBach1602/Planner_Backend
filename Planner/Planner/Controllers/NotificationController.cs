using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    //Notification

    [Route("api/Notification")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Add(NotificationInput notificationInput)
        {
            await _unitOfWork.Notification.AddAsync(notificationInput);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Add Notification successfully" });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            await _unitOfWork.Notification.UpdateStatus(id, status);
            if (status == "Accepted")
            {
                var notification = await _unitOfWork.Notification.GetAsync(x => x.Id == id);
                if (notification != null)
                {
                    await _unitOfWork.UserPlan.AddUserToPlan(notification.PlanId, notification.ReceivedUserId, "Member");
                }
            }


            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Update Notification succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("isSeen/{id}")]
        public async Task<IActionResult> UpdateIsSeen(int id)
        {
            await _unitOfWork.Notification.UpdateIsSeen(id);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Update Notification succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            var Notifications = await _unitOfWork.Notification.GetAllAsync(x => x.ReceivedUserId == userId);
            if (Notifications == null)
            {
                return NotFound();
            }

            return Ok(Notifications.OrderByDescending(x => x.Id));

        }
    }
}
