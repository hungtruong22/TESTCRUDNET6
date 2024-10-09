using Microsoft.AspNetCore.Identity;
using TESTCRUDNET6.Models;

namespace TESTCRUDNET6.Services
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);

        public Task<string> SignInAsync(SignInModel model);
    }
}
