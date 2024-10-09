namespace TESTCRUDNET6.DTOs
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
