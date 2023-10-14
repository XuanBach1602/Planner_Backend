using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Text.RegularExpressions;

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
                return Ok(new {message = "Update plan succesfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);         
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var  plans = await _unitOfWork.Plan.GetAllAsync();
            if(plans == null)
            {
                return NotFound();
            }
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
            if(plan == null)
            {
                return NotFound();
            }
             _unitOfWork.Plan.Remove(plan);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Delele plan successfully" });
            }
            catch(Exception ex)
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
            var plan = await _unitOfWork.Plan.GetFirstOrDefaultAsync(x => x.Id==id);
            if(plan == null)
            {
                return NotFound();
            }
            return Ok(plan);
        }

        [HttpGet("{planID}/tasks")]
        public async Task<IActionResult> GetTasksByPlanID(int? planID)
        {
            if(planID == null)
            {
                return BadRequest("PlanID is required");
            }

            var tasks = await _unitOfWork.WorkTask.GetAllAsync(x => x.PlanID==planID);
            return Ok(tasks);
        }
       
    }
}
