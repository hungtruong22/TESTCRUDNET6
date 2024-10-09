using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TESTCRUDNET6.Models;
using TESTCRUDNET6.Services;

namespace TESTCRUDNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository) 
        {
            _repository = repository;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _repository.SignUpAsync(model);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SingIn(SignInModel model)
        {
            var result = await _repository.SignInAsync(model);


            // trường hợp đăng nhập không thành công
            if (string.IsNullOrEmpty(result)) 
            {
                return Unauthorized();
            }

            return Ok(result);

        }
    }
}
