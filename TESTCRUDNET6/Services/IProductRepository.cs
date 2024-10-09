using TESTCRUDNET6.DTOs;
using TESTCRUDNET6.Models;

namespace TESTCRUDNET6.Services
{
    public interface IProductRepository
    {
        List<ProductModel> GetAll();
        ProductModel GetById(string id);
        Task<ProductModel> AddProductAsync(ProductCreateDTO product);
        Task Update(ProductUpdateDTO product);
        void Delete(string id);
        List<ProductModel> GetProductByLoai(int categoryId);
        List<ProductModel> GetAllFilter(string? search, double? from, double? to, string sortBy, int page);
        PagiproResponse getProducts(string? search, int? categoryId, int page = 1);
    }
}
