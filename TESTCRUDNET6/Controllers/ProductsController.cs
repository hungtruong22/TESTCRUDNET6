using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TESTCRUDNET6.DTOs;
using TESTCRUDNET6.Models;
using TESTCRUDNET6.Services;

namespace TESTCRUDNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_productRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var data = _productRepository.GetById(id);
                if(data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductCreateDTO product)
        {
            try
            {
                var result = await _productRepository.AddProductAsync(product);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromForm] ProductUpdateDTO model)
        {
            if(Guid.Parse(id) != model.ProductId)
            {
                return BadRequest();
            }

            try
            {
                await _productRepository.Update(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                _productRepository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("get-product-by-cate/{id}")]
        public IActionResult GetProductByCate(int id)
        {
            try
            {
                return Ok(_productRepository.GetProductByLoai(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("getFilter")]
        public IActionResult GettAllProducts(string? search, double? from, double? to, string? sortBy, int page = 1)
        {
            try
            {
                var result = _productRepository.GetAllFilter(search, from, to, sortBy, page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the product.");
            }
        }

        [HttpGet("getProduct")]
        public IActionResult GetProducts(string? search, int? categoryId, int page = 1)
        {
            try
            {
                var result = _productRepository.getProducts(search, categoryId, page);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Success",
                    Data = result
                });
            }
            catch
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "We can't get the product."
                }); ;
            }
        }
    }
}
