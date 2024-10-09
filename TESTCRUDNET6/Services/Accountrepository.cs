using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TESTCRUDNET6.Data;
using TESTCRUDNET6.Helpers;
using TESTCRUDNET6.Models;

namespace TESTCRUDNET6.Services
{
    public class Accountrepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Accountrepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager) 
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._roleManager = roleManager;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            // trường hợp không tìm thấy user hoặc pasword không hợp lệ 
            if(user == null)
            {
                return string.Empty;
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if(!result.Succeeded) // trường hợp không thành công
            {
                return string.Empty;
            }

            // trường hợp thành công

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            // Tạo key để ký token
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));


            // tạp accessToken
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20), // thời gian hết hạn token là 20 phút
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature) 
                );

            // Tạo refresh token (có thể là một GUID hoặc một chuỗi ngẫu nhiên)
            //var refreshToken = Guid.NewGuid().ToString();

            // Lưu refresh token vào cơ sở dữ liệu cùng với thông tin người dùng
            // Ví dụ: await _refreshTokenRepository.SaveRefreshTokenAsync(model.Email, refreshToken);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {

            if(model.ConfirmPassword.Equals(model.Password))
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                // trường hợp đăng ký thành công 
                if (result.Succeeded)
                {
                    // kiểm tra role Customer đã có hay chưa
                    if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                    }

                    await _userManager.AddToRoleAsync(user, AppRole.Customer);
                }
                return result;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Mật khẩu xác nhận không khớp." });
        }
    }
}
