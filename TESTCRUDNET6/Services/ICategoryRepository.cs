using TESTCRUDNET6.Models;

namespace TESTCRUDNET6.Services
{
    public interface IcategoryRepository
    {
        List<CategoryModel> GetAll();
        CategoryModel GetById(int id);
        CategoryModel Add(CategoryVM category);
        void Update(CategoryModel category);
        void Delete(int id);
        PagiproResponse getCategories(string? search, int page = 1);
    }
}
