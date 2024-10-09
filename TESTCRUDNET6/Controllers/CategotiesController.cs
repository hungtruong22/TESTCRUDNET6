using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TESTCRUDNET6.Models;
using TESTCRUDNET6.Services;

namespace TESTCRUDNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategotiesController : ControllerBase
    {
        private readonly IcategoryRepository _categoryRepository;

        public CategotiesController(IcategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll() {
            try
            {
                return Ok(_categoryRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var data = _categoryRepository.GetById(id);
                if (data != null)
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
        [Authorize]
        public IActionResult AddNewCate(CategoryVM cate)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created ,_categoryRepository.Add(cate));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCate(int id, CategoryModel model)
        {
            if (id != model.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                _categoryRepository.Update(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCate(int id)
        {
            try
            {
                _categoryRepository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
