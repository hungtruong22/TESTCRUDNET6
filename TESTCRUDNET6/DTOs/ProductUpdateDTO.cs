namespace TESTCRUDNET6.DTOs
{
    public class ProductUpdateDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
