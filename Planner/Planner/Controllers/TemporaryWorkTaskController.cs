//using Microsoft.AspNetCore.Mvc;
//using Planner.Model;
//using Planner.Repository.IRepository;
//using System.ComponentModel.DataAnnotations;

//namespace Planner.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TemporaryWorkTaskController : ControllerBase
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        public TemporaryWorkTaskController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var WorkTask = await _unitOfWork.TemporaryWorkTask.FindById(id);
//            if (WorkTask == null)
//            {
//                return NotFound();
//            }

//            return Ok(WorkTask);
//            ;
//        }

//        public class TemporaryWorkTaskInput
//        {
//            public int? Id { get; set; } = 0;
//            public int WorkTaskId;
//            [Required]
//            public string Name { get; set; } = string.Empty;
//            public string? Description { get; set; } = string.Empty;
//            [Required]
//            public string Status { get; set; } = string.Empty;
//            public required string Priority { get; set; }
//            public bool IsPrivate { get; set; }
//            [Required]
//            public DateTime StartDate { get; set; }
//            [Required]
//            public DateTime DueDate { get; set; }

//            [Required]
//            public int CategoryID { get; set; }
//            [Required]
//            public int PlanID { get; set; }
//            [Required]
//            public string CreatedUserID { get; set; } = string.Empty;
//            public string? AssignedUserID { get; set; }
//            public string? CompletedUserID { get; set; }
//            public bool IsApproved { get; set; }
//            public List<IFormFile>? AttachedFiles { get; set; }

//        }
//        private TemporaryWorkTask ConvertToTemporaryWorkTask(TemporaryWorkTaskInput model)
//        {
//            return new TemporaryWorkTask
//            {
//                Id = (int)((model.Id == null) ? 0 : model.Id),
//                Name = model.Name,
//                Description = model.Description,
//                Status = model.Status,
//                CreatedUserID = model.CreatedUserID,
//                AssignedUserID = model.AssignedUserID != "null" ? model.AssignedUserID : null,
//                Priority = model.Priority,
//                StartDate = model.StartDate,
//                DueDate = model.DueDate,
//                CategoryID = model.CategoryID,
//                PlanId = model.PlanID,
//                IsPrivate = model.IsPrivate,
//                CompletedUserId = model.CompletedUserID != "null" ? model.CompletedUserID : null,
//                IsApproved = model.IsApproved,
//                WorkTaskId = model.WorkTaskId,
//            };
//        }

//        private WorkTaskOutput ConvertToFormatted(WorkTask workTask)
//        {
//            return new WorkTaskOutput
//            {
//                Id = workTask.Id,
//                Name = workTask.Name,
//                Description = workTask.Description,
//                Status = workTask.Status,
//                Priority = workTask.Priority,
//                StartDate = workTask.StartDate.ToString("yyyy-MM-dd"),
//                DueDate = workTask.DueDate.ToString("yyyy-MM-dd"),
//                CategoryId = workTask.CategoryID,
//                PlanId = workTask.PlanId,
//                CreatedUserId = workTask.CreatedUserID,
//                AssignedUserId = workTask.AssignedUserID,
//                Files = workTask.Files.ToList(),
//                ModifiedDate = workTask.ModifiedDate.ToString(),
//                CategoryName = workTask.Category.Name,
//                IsPrivate = workTask.IsPrivate,
//                CompletedUserId = workTask.CompletedUserId,
//                CompletedTime = workTask.CompletedTime?.ToString("yyyy-MM-dd"),
//                IsApproved = workTask.IsApproved

//            };
//        }

//        public class TemporaryWorkTaskOutput
//        {
//            public int Id { get; set; }
//            public int WorkTaskId { get; set; }
//            public required string Name { get; set; }
//            public string? Description { get; set; }
//            public required string Status { get; set; }
//            public required string Priority { get; set; }
//            public required string StartDate { get; set; }
//            public required string DueDate { get; set; }
//            public required bool IsPrivate { get; set; }
//            public int CategoryId { get; set; }
//            public string? CompletedUserId { get; set; }
//            public int PlanId { get; set; }
//            public required string CreatedUserId { get; set; }
//            public string? AssignedUserId { get; set; }
//            public string ModifiedDate { get; set; }
//            public string? CompletedTime { get; set; }
//            public bool IsApproved { get; set; }
//            public List<UploadFile> Files { get; set; }
//            public string CategoryName { get; set; } = string.Empty;
//        }
//    }
//}
