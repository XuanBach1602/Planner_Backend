using Planner.Repository.IRepository;

namespace Planner.Repository
{
    public class FileService : IFileService
    {
        public async Task<Byte[]> DownloadFileById(string fileName)
        {
            try
            {
                string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles"));
                string filePath = Path.Combine(path, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    // Đọc nội dung tệp thành một mảng byte
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return fileBytes;
                }
                else
                {
                    // Xử lý trường hợp tệp không tồn tại
                    throw new FileNotFoundException("File not found");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                throw new Exception("File Download Failed: " + ex.Message);
            }
        }

        public async Task<string> PostMultiFileAsync(List<IFormFile> fileData)
        {
            List<string> fileUrls = new List<string>();
            try
            {
                foreach (IFormFile file in fileData)
                {
                    string urlFile = await UploadFile(file);
                    if (!string.IsNullOrEmpty(urlFile))
                    {
                        fileUrls.Add(urlFile);
                    }
                }

                return string.Join(",", fileUrls);
            }
            catch (Exception ex)
            {
                throw new Exception("File Upload Failed: " + ex.Message);
            }
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file != null)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string uniqueFileName = GetUniqueFileName(file.FileName, path);
                    string filePath = Path.Combine(path, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return filePath;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        private string GetUniqueFileName(string fileName, string path)
        {
            string uniqueFilename = fileName;
            int count = 1;
            while (System.IO.File.Exists(Path.Combine(path, uniqueFilename)))
            {
                uniqueFilename = Path.GetFileNameWithoutExtension(uniqueFilename) + "_" + count + Path.GetExtension(fileName);
                count++;
            }
            return uniqueFilename;
        }
    }
}
