using System.Threading.Tasks;
using Unsplash.Core.ApiModels;

namespace Unsplash.Core.Services.Interfaces
{
     public interface IAuthService
    {
        Task<TokenModel> LogUserIn(LoginModel model);
        Task<(UserModel user, string message)> RegisterUser(SignUpModel model);
        (object user, string message) LogUserOut();

    }
}