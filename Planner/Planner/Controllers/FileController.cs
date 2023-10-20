using Microsoft.AspNetCore.Mvc;
using Planner.Repository.IRepository;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var fileBytes = await _fileService.DownloadFileById(fileName);
            if (fileBytes != null)
            {
                // Trả về tệp tải xuống với loại nội dung và tên tệp
                return Ok(File(fileBytes, "application/octet-stream", fileName));
            }
            else
            {
                // Xử lý trường hợp tệp không tồn tại
                return NotFound("File not found");
            }

        }
    }
}
