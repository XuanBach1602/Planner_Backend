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
            var WorkTaskFormatted = WorkTasks.Select(ConvertToFormatted).ToList();
            return Ok(WorkTaskFormatted);

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
            var WorkTaskFormatted = ConvertToFormatted(WorkTask);
            return Ok(WorkTaskFormatted);
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
            var WorkTaskFormatted = worktasks.Select(ConvertToFormatted).ToList();
            return Ok(WorkTaskFormatted);
        }

        [HttpGet("GetByCategoryID/{categoryID}")]
        public async Task<IActionResult> GetTasksByCategoryID(int? categoryID)
        {
            if (categoryID == null)
            {
                return BadRequest("Category is required");
            }

            var tasks = await _unitOfWork.WorkTask.GetAllAsync(x => x.CategoryID == categoryID);
            var WorkTaskFormatted = tasks.Select(ConvertToFormatted).ToList();
            return Ok(WorkTaskFormatted);
        }
        public class WorkTaskFormatted
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public string? Description { get; set; }
            public required string Status { get; set; }
            public required string Priority { get; set; }
            public required string StartDate { get; set; }
            public required string DueDate { get; set; }
            public string? Attachment { get; set; }
            public int CategoryId { get; set; }
            public required string CreatedUserId { get; set; }
            public string? AssignedUserId { get; set; }
            // Other properties if needed
        }

        private WorkTaskFormatted ConvertToFormatted(WorkTask workTask)
        {
            return new WorkTaskFormatted
            {
                Id = workTask.Id,
                Name = workTask.Name,
                Description = workTask.Description,
                Status = workTask.Status,
                Priority = workTask.Priority,
                StartDate = workTask.StartDate.ToString("yyyy-MM-dd"),
                DueDate = workTask.DueDate.ToString("yyyy-MM-dd"),
                Attachment = workTask.Attachment,
                CategoryId = workTask.CategoryID,
                CreatedUserId = workTask.CreatedUserID,
                AssignedUserId = workTask.AssignedUserID,
            };
        }

    }


}
