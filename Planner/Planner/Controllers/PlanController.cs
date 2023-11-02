using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    [Route("api/plan")]
    [ApiController]
    public class PlanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Plan plan)
        {
            await _unitOfWork.Plan.AddAsync(plan);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Add Plan successfully" });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Plan plan)
        {
            _unitOfWork.Plan.Update(plan);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Update plan succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _unitOfWork.Plan.GetAllAsync();
            if (plans == null)
            {
                return NotFound();
            }
            //var PlanModels = plans.Select(u => ConvertPlanToPlanModel(u));
            return Ok(plans);

        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var plan = await _unitOfWork.Plan.GetFirstOrDefaultAsync(x => x.Id == id);
            if (plan == null)
            {
                return NotFound();
            }
            _unitOfWork.Plan.Remove(plan);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Delele plan successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var plan = await _unitOfWork.Plan.GetFirstOrDefaultAsync(x => x.Id == id);
            if (plan == null)
            {
                return NotFound();
            }
            return Ok(plan);
        }


        [HttpGet("GetByUserID/{userID}")]
        public async Task<IActionResult> GetByUserID(string userID)
        {
            if (userID == null)
            {
                return NotFound("UserID is required");
            }
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Id == userID);
            if (user == null)
            {
                return NotFound("User is not exist");
            }

            var plans = await _unitOfWork.Plan.GetPlansByUserID(userID);
            return Ok(plans);
        }

    }
}
