using Planner.Repository.IRepository;
using System.Security.Cryptography;
using System.Text;

namespace Planner.Repository
{
    public class FileService : IFileService
    {
        public async Task<Byte[]> DownloadFileById(string fileName, string pathToFolder, string userId)
        {
            try
            {
                //string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles"));
                var encodedFileName = EncodeFileName(fileName, userId);
                string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, pathToFolder));
                string filePath = Path.Combine(path, encodedFileName);
                if (System.IO.File.Exists(filePath))
                {
                    // Đọc nội dung tệp thành một mảng byte
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

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

        public async Task<string> PostMultiFileAsync(List<IFormFile> fileData, string folderName, string userId)
        {
            List<string> fileUrls = new List<string>();
            try
            {
                foreach (IFormFile file in fileData)
                {
                    string urlFile = await UploadFile(file, folderName, userId);
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

        public async Task<string> UploadFile(IFormFile file, string pathToFIle, string userId)
        {
            string path = "";
            try
            {
                if (file != null)
                {

                    //path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadFiles/Avatar"));
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, pathToFIle));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //string uniqueFileName = GetUniqueFileName(file.FileName, path);
                    var encodedFileName = EncodeFileName(file.FileName, userId);
                    string filePath = Path.Combine(path, encodedFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    return file.FileName;
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



        public string EncodeFileName(string fileName, string userId)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            // Kết hợp tên tệp và Id của người dùng
            string combinedString = fileNameWithoutExtension + userId;

            // Mã hóa chuỗi kết hợp để tạo tên tệp mã hóa
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
            // Giải mã chuỗi tên tệp mã hóa
            byte[] hashBytes = Convert.FromBase64String(encodedFileName);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] combinedBytes = sha256.ComputeHash(hashBytes);
                string combinedString = Encoding.UTF8.GetString(combinedBytes);

                // Kiểm tra xem userId khớp với userId trong tên tệp đã giải mã
                if (combinedString.EndsWith(userId))
                {
                    // Lấy tên tệp gốc bằng cách loại bỏ Id của người dùng
                    string originalFileName = combinedString.Substring(0, combinedString.Length - userId.Length);
                    return originalFileName;
                }
            }

            // Trong trường hợp không hợp lệ hoặc userId không khớp, trả về null hoặc xử lý khác tùy theo trường hợp
            return null;
        }


        public bool DeleleteFile(string fileName, string folderName, string userId)
        {
            var decodedFileName = EncodeFileName(fileName, userId);
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, folderName));
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
            return false;
        }
        public bool DeleteMultipleFiles(List<IFormFile> fileData, string folderName, string userId)
        {
            try
            {
                foreach (IFormFile file in fileData)
                {
                    if (!DeleleteFile(file.FileName, folderName, userId)) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
        }

        //public async Task<File> GetFile(string path)
        //{
        //    var imgUrl = Path.Combine(Environment.CurrentDirectory, path);

        //    if (File.Exists(imgUrl))
        //    {
        //        var imgBytes = await File.ReadAllBytesAsync(imgUrl);
        //        return FileMode(imgBytes, "image/jpeg");
        //    }

        //    return new File() // Trả về lỗi 404 nếu không tìm thấy hình ảnh.
        //}

    }
}
