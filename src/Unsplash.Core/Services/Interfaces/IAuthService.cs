using System.Threading.Tasks;
using Unsplash.Core.ApiModels;

namespace Unsplash.Core.Services.Interfaces
{
     public interface IAuthService
    {
        Task<TokenModel> LogUserIn(LoginModel model);
        Task<(UserModel user, string message)> RegisterUser(SignUpModel model);
        Task<(object user, string message)> LogUserOut();

    }
}