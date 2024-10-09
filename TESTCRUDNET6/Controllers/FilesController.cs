using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TESTCRUDNET6.Services;

namespace TESTCRUDNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService) 
        {
            _fileService = fileService;
        }


        // Gọi tới phương thức lưu file từ service
        [HttpPost("upload-stream")]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            try
            {
                var filePath = await _fileService.SaveFileAsync(file);
                return Ok(new { filePath });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Gọi tới phương thức tải file từ service
        [HttpGet("download-stream/{fileName}")]
        public IActionResult LoadFile(string fileName)
        {
            try
            {
                var fileStream = _fileService.LoadFile(fileName);
                var contentType = "application/octet-stream";
                return new FileStreamResult(fileStream, contentType)
                {
                    FileDownloadName = fileName
                };
            }
            catch (FileNotFoundException)
            {
                return NotFound("File không tồn tại.");
            }
        }
    }
}
