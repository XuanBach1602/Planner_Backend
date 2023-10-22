using Microsoft.AspNetCore.Mvc;
using Planner.Model;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    //Category

    [Route("api/Category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category Category)
        {
            await _unitOfWork.Category.AddAsync(Category);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Add Category successfully" });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Category Category)
        {
            _unitOfWork.Category.Update(Category);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Update Category succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Categorys = await _unitOfWork.Category.GetAllAsync();
            if (Categorys == null)
            {
                return NotFound();
            }

            return Ok(Categorys);

        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var Category = await _unitOfWork.Category.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(Category);
            try
            {
                await _unitOfWork.Save();
                return Ok(new { message = "Delele Category successfully" });
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
            var Category = await _unitOfWork.Category.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            return Ok(Category);
            ;
        }

        [HttpGet("GetByPlanID/{planID}")]
        public async Task<IActionResult> GetTasksByPlanID(int? planID)
        {
            if (planID == null)
            {
                return BadRequest("PlanID is required");
            }

            var tasks = await _unitOfWork.Category.GetAllAsync(x => x.PlanID == planID);
            return Ok(tasks);
        }

    }
}
