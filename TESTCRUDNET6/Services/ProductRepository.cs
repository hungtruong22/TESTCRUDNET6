using Microsoft.EntityFrameworkCore;
using TESTCRUDNET6.Data;
using TESTCRUDNET6.DTOs;
using TESTCRUDNET6.Models;

namespace TESTCRUDNET6.Services
{
    public class ProductRepository : IProductRepository
    {
        public static int PAGE_SIZE { get; set; } = 5;
        private readonly MyDbContext _context;
        private readonly IFileService _fileService;

        public ProductRepository(MyDbContext context, IFileService fileService) 
        {
            _context = context;
            _fileService = fileService;
        }

        /*public ProductModel Add(ProductVM product)
        {
            var _product = new Product
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
            };

            _context.Add(_product);
            _context.SaveChanges();

            return new ProductModel
            {
                ProductName = _product.ProductName,
                Price = _product.Price,
                Description = _product.Description,
                CategoryId = _product.CategoryId,
            };
        }*/

        public async Task<ProductModel> AddProductAsync(ProductCreateDTO product)
        {
            string Image = await _fileService.SaveFileAsync(product.Image);

            var _product = new Product
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                Image = Image,
                CategoryId = product.CategoryId,
            };

            await _context.AddAsync(_product);
            await _context.SaveChangesAsync();

            return new ProductModel
            {
                ProductName = _product.ProductName,
                Price = _product.Price,
                Description = _product.Description,
                Image = _product.Image,
                CategoryId = _product.CategoryId,
            };
        }


        public void Delete(string id)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == Guid.Parse(id));
            if (product != null)
            {
                _context.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<ProductModel> GetAll()
        {
            var productList = _context.Products.Select(p => new ProductModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Image = p.Image,
                Description = p.Description,
                CategoryId = p.CategoryId,
            });

            return productList.ToList();
        }

        public ProductModel GetById(string id)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == Guid.Parse(id));
            if(product != null)
            {
                return new ProductModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Image = product.Image,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                };
            }
            return null;
        }

        public List<ProductModel> GetProductByLoai(int categoryId)
        {
            var list = _context.Products
        .Where(p => p.CategoryId == categoryId)
            .Select(p => new ProductModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Description = p.Description,
                Image = p.Image,
                CategoryId = p.CategoryId,
            });

            return list.ToList();
        }

        public async Task Update(ProductUpdateDTO product)
        {
            var _product = _context.Products.SingleOrDefault(p => p.ProductId == product.ProductId);
            string ImagePath = "";

            if(product.Image != null)
            {
                ImagePath = await _fileService.SaveFileAsync(product.Image);
            }

            _product.ProductName = product.ProductName;
            _product.Price = product.Price;
            _product.Image = ImagePath;
            _product.Description = product.Description;
            _product.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();   
        }

        public List<ProductModel> GetAllFilter(string search, double? from, double? to, string sortBy, int page = 1)
        {
            var allProducts = _context.Products.Include(hh => hh.Category).AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts.Where(hh => hh.ProductName.Contains(search));
            }
            if (from.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.Price >= from);
            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.Price <= to);
            }
            #endregion


            #region Sorting
            //Default sort by Name (TenHh)
            allProducts = allProducts.OrderBy(hh => hh.ProductName);

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "tenhh_desc": allProducts = allProducts.OrderByDescending(hh => hh.ProductName); break;
                    case "gia_asc": allProducts = allProducts.OrderBy(hh => hh.ProductName); break;
                    case "gia_desc": allProducts = allProducts.OrderByDescending(hh => hh.ProductName); break;
                }
            }
            #endregion

            //#region Paging
            //allProducts = allProducts.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            //#endregion

            //var result = allProducts.Select(hh => new HangHoaModel
            //{
            //    MaHangHoa = hh.MaHh,
            //    TenHangHoa = hh.TenHh,
            //    DonGia = hh.DonGia,
            //    TenLoai = hh.Loai.TenLoai
            //});

            //return result.ToList();

            var result = PaginatedList<Product>.Create(allProducts, page, PAGE_SIZE);

            return result.Select(hh => new ProductModel
            {
                ProductId = hh.ProductId,
                ProductName = hh.ProductName,
                Price = hh.Price,
                Description = hh.Description,
                CategoryId = hh.CategoryId,
            }).ToList();
        }

        public PagiproResponse getProducts(string? search, int? categoryId, int page = 1)
        {
            var allProducts = _context.Products.Include(hh => hh.Category).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts.Where(hh => hh.ProductName.Contains(search));
            }

            if (categoryId.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.CategoryId == categoryId);
            }
            

            var result = PaginatedList<Product>.Create(allProducts, page, PAGE_SIZE);
            int totalPage = result.TotalPage;

            /*return result.Select(hh => new ProductModel
            {
                ProductId = hh.ProductId,
                ProductName = hh.ProductName,
                Price = hh.Price,
                Description = hh.Description,
                CategoryId = hh.CategoryId,
            }).ToList();*/

            var list = result.Select(hh => new ProductModel
            {
                ProductId = hh.ProductId,
                ProductName = hh.ProductName,
                Price = hh.Price,
                Image = hh.Image,
                Description = hh.Description,
                CategoryId = hh.CategoryId,
            }).ToList();

            PagiproResponse response = new PagiproResponse { 
                totalPage = totalPage,
                Data = list
            };

            return response;
            
        }

        
    }
}
