using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planner.Repository.IRepository;
using System.Security.Claims;

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
        [HttpGet("avatar/{fileName}")]
        [Authorize]
        public async Task<IActionResult> GetAvatar(string fileName)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                // Sử dụng userId để xử lý tên file và tải ảnh
                var fileBytes = await _fileService.DownloadFileById(fileName, "UploadFiles/Avatars", userId);

                if (fileBytes != null)
                {
                    return File(fileBytes, "image/jpeg");
                }
            }

            return NotFound("File not found");
        }
        [Authorize]
        [HttpGet("document/{fileName}")]
        public async Task<IActionResult> GetDocument(string fileName)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var fileBytes = await _fileService.DownloadFileById(fileName, "UploadFiles/Documents", userId);
                if (fileBytes != null)
                {
                    string contentType = GetContentType(fileName);
                    return File(fileBytes, contentType);
                }
                else
                {
                    return NotFound("File not found");
                }
            }

            return NotFound();


        }

        private string GetContentType(string fileName)
        {
            // Xác định loại dữ liệu dựa trên phần mở rộng của tên tệp
            string fileExtension = Path.GetExtension(fileName).ToLower();

            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                default:
                    return "application/octet-stream"; // Loại mặc định nếu không xác định được
            }
        }
    }
}
