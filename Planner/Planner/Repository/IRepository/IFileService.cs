namespace Planner.Repository.IRepository
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile fileData, string pathToFile, string userId);
        public Task<string> PostMultiFileAsync(List<IFormFile> fileData, string folderName, string userId);
        public Task<Byte[]> DownloadFileById(string fileName, string pathToFolder, string userId);
        public bool DeleleteFile(string fileName, string folderName, string userId);
        public bool DeleteMultipleFiles(List<IFormFile> fileData, string folderName, string userId);
    }
}
