using System.Security.Cryptography;
using System.Text;

namespace Planner.Services
{
    public class FileService : IFileService
    {
        private string folderName = "UploadFiles";
        public async Task<byte[]> DownloadFileByUrl(string url)
        {
            try
            {
                //string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles"));
                //var encodedFileName = EncodeFileName(fileName, userId);
                string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, folderName));
                string filePath = Path.Combine(path, url);
                if (File.Exists(filePath))
                {
                    // Đọc nội dung tệp thành một mảng byte
                    byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

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

        //public async Task<string> PostMultiFileAsync(List<IFormFile> fileData, string folderName, string userId)
        //{
        //    if (fileData == null) return string.Empty;
        //    List<string> fileUrls = new List<string>();
        //    try
        //    {
        //        foreach (IFormFile file in fileData)
        //        {
        //            string urlFile = await UploadFile(file, folderName, userId);
        //            if (!string.IsNullOrEmpty(urlFile))
        //            {
        //                fileUrls.Add(urlFile);
        //            }
        //        }

        //        return string.Join(",", fileUrls);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("File Upload Failed: " + ex.Message);
        //    }
        //}

        public async Task<string> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file != null)
                {

                    //path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles/Avatar"));
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var uniqueFileName = GenerateUniqueFileNameWithExtension(file.FileName);
                    //var encodedFileName = EncodeFileName(file.FileName, userId);
                    string filePath = Path.Combine(path, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    return uniqueFileName;
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

        public string GenerateUniqueFileNameWithExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            Guid uniqueGuid = Guid.NewGuid();
            string uniqueName = uniqueGuid.ToString();
            return uniqueName + extension;
        }


        public string EncodeFileName(string fileName, string userId)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string combinedString = fileNameWithoutExtension + userId;
            byte[] combinedBytes = Encoding.UTF8.GetBytes(combinedString);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string encodedFileName = Convert.ToBase64String(hashBytes);
                return encodedFileName + extension;
            }
        }

        public string DecodeFileName(string encodedFileName, string userId)
        {
            byte[] hashBytes = Convert.FromBase64String(encodedFileName);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] combinedBytes = sha256.ComputeHash(hashBytes);
                string combinedString = Encoding.UTF8.GetString(combinedBytes);

                if (combinedString.EndsWith(userId))
                {
                    string originalFileName = combinedString.Substring(0, combinedString.Length - userId.Length);
                    return originalFileName;
                }
            }

            // Trong trường hợp không hợp lệ hoặc userId không khớp, trả về null hoặc xử lý khác tùy theo trường hợp
            return null;
        }


        public bool DeleteFile(string url)
        {
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, folderName, url));
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path); // Xóa tệp nếu nó tồn tại
                    return true;
                }
                else
                {
                    Console.WriteLine($"File not found: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
            return false;
        }

        //public bool DeleteMultipleFiles(string fileNameList)
        //{
        //    try
        //    {
        //        var fileNameArray = fileNameList.Split(",");
        //        foreach (string fileName in fileNameArray)
        //        {
        //            if (!DeleleteFile(fileName, folderName, userId)) return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error deleting file: {ex.Message}");
        //        return false;
        //    }
        //}

    }
}
