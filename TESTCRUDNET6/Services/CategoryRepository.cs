using TESTCRUDNET6.Data;
using TESTCRUDNET6.Models;

namespace TESTCRUDNET6.Services
{
    public class CategoryRepository : IcategoryRepository
    {
        private readonly MyDbContext _context;

        public CategoryRepository(MyDbContext context) 
        {
            _context = context;
        }

        public CategoryModel Add(CategoryVM category)
        {
            var _category = new Category
            {
                CategoryName = category.CategoryName
            };

             _context.Add(_category);
            _context.SaveChanges();

            return new CategoryModel
            {
                CategoryId = _category.CategoryId,
                CategoryName = _category.CategoryName
            };
        }

        public void Delete(int id)
        {
            var _category = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
            if( _category != null )
            {
                _context.Remove(_category );
                _context.SaveChanges();
            }
        }

        public List<CategoryModel> GetAll()
        {
            var categoryList = _context.Categories.Select(c => new CategoryModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            });

            return categoryList.ToList();
        }

        public CategoryModel GetById(int id)
        {
            var _category = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
            if( _category != null )
            {
                return new CategoryModel
                {
                    CategoryId = _category.CategoryId,
                    CategoryName = _category.CategoryName
                };
            }
            return null;
        }

        public void Update(CategoryModel category)
        {
            var _category = _context.Categories.SingleOrDefault(c => c.CategoryId == category.CategoryId);
              
            _category.CategoryName = category.CategoryName;
            _context.SaveChanges();
        }
    }
}
