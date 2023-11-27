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
            var workTask = ConvertToWorkTask(workTaskInput);
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
            var workTask = ConvertToWorkTask(workTaskInput);
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

        [HttpPut("Status/{id}")]
        public async Task<IActionResult> StatusUpdate(int id, string status, string? userId)
        {
            await _unitOfWork.WorkTask.UpdateStatus(id, status, userId);
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
            await _unitOfWork.WorkTask.Remove(id);
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

        public class WorkTaskInput
        {
            public int? Id { get; set; } = 0;
            [Required]
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;
            [Required]
            public string Status { get; set; } = string.Empty;
            public required string Priority { get; set; }
            public bool IsPrivate { get; set; }
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
            public string? CompletedUserID { get; set; }
            public bool IsApproved { get; set; }
            public List<IFormFile>? AttachedFiles { get; set; }

        }
        private WorkTask ConvertToWorkTask(WorkTaskInput model)
        {
            return new WorkTask
            {
                Id = (int)((model.Id == null) ? 0 : model.Id),
                Name = model.Name,
                Description = model.Description,
                Status = model.Status,
                CreatedUserID = model.CreatedUserID,
                AssignedUserID = model.AssignedUserID != "null" ? model.AssignedUserID : null,
                Priority = model.Priority,
                StartDate = model.StartDate,
                DueDate = model.DueDate,
                CategoryID = model.CategoryID,
                PlanId = model.PlanID,
                IsPrivate = model.IsPrivate,
                CompletedUserId = model.CompletedUserID != "null" ? model.CompletedUserID : null,
                IsApproved = model.IsApproved
            };
        }
    }


}
