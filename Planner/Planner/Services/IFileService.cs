namespace Planner.Services
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile fileData);
        //public Task<string> PostMultiFileAsync(List<IFormFile> fileData, string folderName, string userId);
        public Task<byte[]> DownloadFileByUrl(string url);
        public bool DeleteFile(string url);
        //public bool DeleteMultipleFiles(string fileNameList, string folderName, string userId);
    }
}
