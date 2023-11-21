using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;
using Planner.Services;
using System.ComponentModel.DataAnnotations;

namespace Planner.Controllers
{
    //Checked

    [Route("api/worktask")]
    [ApiController]
    public class WorkTaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilterService _filterService;

        public WorkTaskController(IUnitOfWork unitOfWork, IFilterService filterService)
        {
            _unitOfWork = unitOfWork;
            _filterService = filterService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] WorkTaskInput workTaskInput)
        {
            var workTask = await ConvertToWorkTask(workTaskInput);
            await _unitOfWork.WorkTask.AddAsync(workTask);
            try
            {
                await _unitOfWork.Save();
                var fileList = workTaskInput.AttachedFiles;
                await _unitOfWork.UploadFile.AddMultipleFiles(fileList, workTask.Id);
                await _unitOfWork.Save();
                return Ok(new { message = "Add workTask successfully" });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] WorkTaskInput workTaskInput)
        {
            var workTask = await ConvertToWorkTask(workTaskInput);
            _unitOfWork.WorkTask.Update(workTask);
            try
            {
                var fileList = workTaskInput.AttachedFiles;
                await _unitOfWork.UploadFile.AddMultipleFiles(fileList, workTask.Id);
                _unitOfWork.UploadFile.DeleteMultipleFile(workTask.Id);
                await _unitOfWork.Save();
                return Ok(new { message = "Update workTask succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Status/{Id}")]
        public async Task<IActionResult> StatusUpdate(int Id, [FromBody] string status)
        {
            var workTask = await _unitOfWork.WorkTask.GetFirstOrDefaultAsync(x => x.Id == Id);
            workTask.Status = status;
            try
            {
                await _unitOfWork.Save();
                return Ok("Update status successfully");
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
            //var WorkTaskFormatted = WorkTasks.Select(ConvertToFormatted).ToList();
            return Ok(WorkTasks);

        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var WorkTask = await _unitOfWork.WorkTask.GetFirstOrDefaultAsync(x => x.Id == id);
            if (WorkTask == null)
            {
                return NotFound();
            }
            _unitOfWork.WorkTask.Remove(WorkTask);
            _unitOfWork.UploadFile.DeleteMultipleFile(id);
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
            //var WorkTaskFormatted = worktasks.Select(ConvertToFormatted).ToList();
            return Ok(worktasks);
        }

        [HttpGet("GetByCategoryID/{categoryID}")]
        public async Task<IActionResult> GetTasksByCategoryID(int? categoryID)
        {
            if (categoryID == null)
            {
                return BadRequest("Category is required");
            }

            var tasks = await _unitOfWork.WorkTask.GetAllAsync(x => x.CategoryID == categoryID);
            //var WorkTaskFormatted = tasks.Select(ConvertToFormatted).ToList();
            return Ok(tasks);
        }

        [HttpGet("GetByPlanID/{planID}")]
        public async Task<IActionResult> GetTasksByPlanID(int planID, string? due, string? priority, string? progress)
        {
            var tasks = await _unitOfWork.WorkTask.GetAllAsync(x => x.PlanId == planID);
            tasks = _filterService.FilterWorkTask(tasks, due, priority, progress);
            //var workTaskFormatted = tasks.Select(ConvertToFormatted).ToList();
            return Ok(tasks);
        }

        [HttpGet("GetCountOfFilteredTask/{planID}")]
        public async Task<IActionResult> GetCountOfFilteredTask(int planID)
        {
            var count = await _unitOfWork.WorkTask.GetCountOfFilteredTask(planID);
            return Ok(count);
        }
        public class WorkTaskOutput
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public string? Description { get; set; }
            public required string Status { get; set; }
            public required string Priority { get; set; }
            public required string StartDate { get; set; }
            public required string DueDate { get; set; }
            public int CategoryId { get; set; }
            public int PlanId { get; set; }
            public required string CreatedUserId { get; set; }
            public string? AssignedUserId { get; set; }
            public string ModifiedDate { get; set; } = string.Empty;
            public List<UploadFile> Files { get; set; } = new List<UploadFile>();
            //public string CategoryName { get; set; } = string.Empty;
            // Other properties if needed
        }

        private WorkTaskOutput ConvertToFormatted(WorkTask workTask)
        {
            //var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(x => x.Id == workTask.CategoryID);
            return new WorkTaskOutput
            {
                Id = workTask.Id,
                Name = workTask.Name,
                Description = workTask.Description,
                Status = workTask.Status,
                Priority = workTask.Priority,
                StartDate = workTask.StartDate.ToString("yyyy-MM-dd"),
                DueDate = workTask.DueDate.ToString("yyyy-MM-dd"),
                CategoryId = workTask.CategoryID,
                PlanId = workTask.PlanId,
                CreatedUserId = workTask.CreatedUserID,
                AssignedUserId = workTask.AssignedUserID,
                Files = _unitOfWork.UploadFile.GetFilesByWorkTaskID(workTask.Id),
                ModifiedDate = workTask.ModifiedDate.ToString()
                //CategoryName = category.Name
            };
        }

        public class WorkTaskInput
        {
            public int? Id { get; set; } = 0;
            [Required]
            public string Name { get; set; } = string.Empty;
            [Required]
            public string Description { get; set; } = string.Empty;
            [Required]
            public string Status { get; set; } = string.Empty;
            [Required]
            public DateTime StartDate { get; set; }
            [Required]
            public DateTime DueDate { get; set; }

            [Required]
            public int CategoryID { get; set; }
            [Required]
            public int PlanID { get; set; }
            [Required]
            public string CreatedUserID { get; set; } = string.Empty;
            public string? AssignedUserID { get; set; }
            public List<IFormFile>? AttachedFiles { get; set; }

        }
        private async Task<WorkTask> ConvertToWorkTask(WorkTaskInput model)
        {
            return new WorkTask
            {
                Id = (int)((model.Id == null) ? 0 : model.Id),
                Name = model.Name,
                Description = model.Description,
                Status = model.Status,
                CreatedUserID = model.CreatedUserID,
                AssignedUserID = model.AssignedUserID,
                //Attachment = await _fileService.PostMultiFileAsync(model.AttachedFiles, "UploadFiles/Documents", model.CreatedUserID),
                StartDate = model.StartDate,
                DueDate = model.DueDate,
                CategoryID = model.CategoryID,
                PlanId = model.PlanID

            };
        }
    }


}
