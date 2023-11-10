namespace Planner.Repository.IRepository
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile fileData);
        //public Task<string> PostMultiFileAsync(List<IFormFile> fileData, string folderName, string userId);
        public Task<Byte[]> DownloadFileByUrl(string url);
        public bool DeleteFile(string url);
        //public bool DeleteMultipleFiles(string fileNameList, string folderName, string userId);
    }
}
