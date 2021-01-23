using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unsplash.Core.ApiModels;
using Unsplash.Core.DataAccess;
using Unsplash.Core.Models;
using Unsplash.Core.Services.Interfaces;

namespace Unsplash.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<User> _userrepo;
        private readonly IRepository<Category> _catrepo;

        private readonly ILogger<CategoryService> log;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(IRepository<User> urepository, IRepository<Category> crepository, ILogger<CategoryService> log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userrepo = urepository;
            _catrepo = crepository;
            this.log = log;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(object response, string message)> CreateCAtegory(CreateCategoryModel model)
        {
            if (model == null) return (response: null, message: "Failed. Check details passed.");
            var createCat = new CreateCategoryModel().PopulateCat(model);
            try
            {
                var newCat = await _catrepo.Insert(createCat);
                return (response: createCat, message: $"Successful. Category - {newCat.CategoryName} created.");
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred while creating category. {ex.Message}");
                return (response: null, message: "Failed. An error occurred.");

            }

        }

        public Task<(object response, string message)> FilterCategory(CategoryModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<(object response, string message)> GetCategories()
        {
            throw new System.NotImplementedException();
        }

        public Task<(object response, string message)> GetCategory(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}