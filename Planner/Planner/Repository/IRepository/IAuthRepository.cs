using Microsoft.AspNetCore.Identity;
using Planner.Model;

namespace Planner.Repository.IRepository
{
    public interface IAuthRepository
    {
        public Task<string> SignIn(SignInModel model);
        public Task<IdentityResult> SignUp(SignUpModel model);
        public Task<bool> SignOut();
        //public string RefreshToken();
    }
}
