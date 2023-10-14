using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;
using System.Text.RegularExpressions;

namespace Planner.Controllers
{
    [Route("api/worktask")]
    [ApiController]
    public class WorkTaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkTaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Add(WorkTask workTask)
        {
            await _unitOfWork.WorkTask.AddAsync(workTask);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Add workTask successfully" });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(WorkTask WorkTask)
        {
            _unitOfWork.WorkTask.Update(WorkTask);
            try
            {
                await _unitOfWork.Save();
                return Ok(new {message = "Update workTask succesfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);         
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByPlanID()
        {
            var  WorkTasks = await _unitOfWork.WorkTask.GetAllAsync();
            if(WorkTasks == null)
            {
                return NotFound();
            }
            return Ok(WorkTasks);
            
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var WorkTask = await _unitOfWork.WorkTask.GetFirstOrDefaultAsync(x => x.Id == id);
            if(WorkTask == null)
            {
                return NotFound();
            }
             _unitOfWork.WorkTask.Remove(WorkTask);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Delele WorkTask successfully" });
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
            var WorkTask = await _unitOfWork.WorkTask.GetFirstOrDefaultAsync(x => x.Id==id);
            if(WorkTask == null)
            {
                return NotFound();
            }
            return Ok(WorkTask);
        }
       
    }
}
