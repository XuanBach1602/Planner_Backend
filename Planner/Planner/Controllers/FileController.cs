using Microsoft.AspNetCore.Mvc;
using Planner.Services;

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
        //[HttpGet("avatar/{fileName}")]
        //public async Task<IActionResult> GetAvatar(string url)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (userId != null)
        //    {
        //        // Sử dụng userId để xử lý tên file và tải ảnh
        //        var fileBytes = await _fileService.DownloadFileByUrl(url);

        //        if (fileBytes != null)
        //        {
        //            return File(fileBytes, "image/jpeg");
        //        }
        //    }

        //    return NotFound("File not found");
        //}
        [HttpGet]
        public async Task<IActionResult> GetDocument(string url)
        {
            var fileBytes = await _fileService.DownloadFileByUrl(url);
            if (fileBytes != null)
            {
                string contentType = GetContentType(url);
                return File(fileBytes, contentType);
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
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // Loại docx
                default:
                    return "application/octet-stream"; // Loại mặc định nếu không xác định được
            }
        }

    }
}
