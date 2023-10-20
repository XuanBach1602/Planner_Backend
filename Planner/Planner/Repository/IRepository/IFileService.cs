namespace Planner.Repository.IRepository
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile fileData);
        public Task<string> PostMultiFileAsync(List<IFormFile> fileData);
        public Task<Byte[]> DownloadFileById(string fileName);
    }
}
