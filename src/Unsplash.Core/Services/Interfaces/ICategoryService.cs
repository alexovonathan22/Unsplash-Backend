using System.Threading.Tasks;
using Unsplash.Core.ApiModels;

namespace Unsplash.Core.Services.Interfaces
{
    public interface ICategoryService
    {
         Task<(object response, string message)> FilterCategory(CategoryModel model);
         Task<(object response, string message)> GetCategories();
         Task<(object response, string message)> GetCategory(int id);
         Task<(object response, string message)> CreateCAtegory(CreateCategoryModel model);
    }
}