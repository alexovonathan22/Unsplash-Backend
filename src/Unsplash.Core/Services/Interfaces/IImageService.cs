using System.Threading.Tasks;
using Unsplash.Core.ApiModels;

namespace Unsplash.Core.Services.Interfaces
{
    public interface IImageService
    {
         Task<(object response, string message)> UploadImageCloudinary(ImageModel model);
         Task<(object response, string message)> RetrieveImages();
         Task<(object response, string message)> GetImage(int id);
         Task<(object response, string message)> SearchImageByTagline(string tagline);
         Task<(object response, string message)> SearchImageByName(string name);

    }
}