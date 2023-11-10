using Planner.Model;

namespace Planner.Repository.IRepository
{
    public interface IUploadFileRepository
    {
        Task AddFile(UploadFile file);
        void DeleteFile(UploadFile file, string url);
        public Task AddMultipleFiles(List<IFormFile> fileList, int worktaskId);
        public void DeleteMultipleFile(int workTaskId);
        public List<UploadFile> GetFilesByWorkTaskID(int workTaskId);

    }
}
