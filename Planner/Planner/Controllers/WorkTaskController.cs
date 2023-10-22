using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    //Checked

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
                return Ok(new { message = "Update workTask succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var WorkTasks = await _unitOfWork.WorkTask.GetAllAsync();
            if (WorkTasks == null)
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
            if (WorkTask == null)
            {
                return NotFound();
            }
            _unitOfWork.WorkTask.Remove(WorkTask);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Delele WorkTask successfully" });
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
            var WorkTask = await _unitOfWork.WorkTask.GetFirstOrDefaultAsync(x => x.Id == id);
            if (WorkTask == null)
            {
                return NotFound();
            }
            return Ok(WorkTask);
            ;
        }

        [HttpGet("GetByUserID/{userID}")]
        public async Task<IActionResult> GetByUserID(string userID)
        {
            if (userID == null)
            {
                return BadRequest("The field ID is required");
            }
            var worktasks = await _unitOfWork.WorkTask.GetAllAsync(x => x.AssignedUserID == userID);
            return Ok(worktasks);
        }

        [HttpGet("GetByCategoryID/{categoryID}")]
        public async Task<IActionResult> GetTasksByPlanID(int? categoryID)
        {
            if (categoryID == null)
            {
                return BadRequest("PlanID is required");
            }

            var tasks = await _unitOfWork.WorkTask.GetAllAsync(x => x.CategoryID == categoryID);
            return Ok(tasks);
        }

        //[NonAction]
        //public WorkTaskModel ConvertWorkTaskToWorkTaskModel(WorkTask WorkTask)
        //{
        //    return new WorkTaskModel
        //    {
        //        Id = WorkTask.Id,
        //        Name = WorkTask.Name,
        //        Description = WorkTask.Description,
        //        Status = WorkTask.Status,
        //        StartDate = WorkTask.StartDate,
        //        DueDate = WorkTask.DueDate,
        //        PlanID = WorkTask.PlanID,
        //        CreatedUserID = WorkTask.CreatedUserID,
        //        AssignedUserID = WorkTask.AssignedUserID
        //    };
        //}

    }
}
