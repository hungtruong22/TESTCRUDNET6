namespace TESTCRUDNET6.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Xử lý lưu file lên server
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ.");
            }

            var uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Tạo tên file duy nhất để tránh ghi đè
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName); // Tạo tên file duy nhất
            var filePath = Path.Combine(uploadPath, fileName);

            // Lưu file bằng stream
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            //return $"/Uploads/{fileName}"; // Trả về đường dẫn file
            return fileName;
        }

        // Xử lý tải file từ server
        public FileStream LoadFile(string fileName)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("File không tồn tại.");
            }

            // Trả về FileStream để đọc file
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
    }
}
