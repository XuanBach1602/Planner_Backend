using Planner.Model;
using Planner.Repository.IRepository;
using Planner.Services;

namespace Planner.Repository
{
    public class UploadFileRepository : IUploadFileRepository
    {
        private readonly PlannerDbContext _context;
        private readonly IFileService _fileService;
        public UploadFileRepository(PlannerDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task AddFile(UploadFile file)
        {
            await _context.AddAsync(file);
        }

        public void DeleteFile(UploadFile file, string url)
        {
            _fileService.DeleteFile(url);
            _context.Remove(file);
        }

        public void DeleteMultipleFile(int workTaskId)
        {
            List<UploadFile> fileList = _context.UploadFiles.Where(x => x.WorkTaskId == workTaskId).ToList();
            foreach (UploadFile file in fileList)
            {
                _fileService.DeleteFile(file.Url);
                _context.Remove(file);
            }
        }

        public async Task AddMultipleFiles(List<IFormFile> fileList, int worktaskId)
        {
            if (fileList != null)
            {
                foreach (var file in fileList)
                {
                    var uploadFile = new UploadFile
                    {
                        Name = file.FileName,
                        Url = await _fileService.UploadFile(file),
                        WorkTaskId = worktaskId
                    };
                    await _context.AddAsync(uploadFile);
                }
            }
        }

        public List<UploadFile> GetFilesByWorkTaskID(int workTaskId)
        {
            var listTask = _context.UploadFiles.Where(x => x.WorkTaskId == workTaskId).ToList();
            return listTask;
        }
    }
}
