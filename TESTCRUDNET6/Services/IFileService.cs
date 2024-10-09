namespace TESTCRUDNET6.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
        FileStream LoadFile(string fileName);
    }
}
