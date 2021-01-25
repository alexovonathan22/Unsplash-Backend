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
        private readonly IRepository<Photo> _imgRepo;
        private readonly ILogger<CategoryService> log;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(IRepository<User> urepository, IRepository<Category> crepository, ILogger<CategoryService> log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IRepository<Photo> imgRepo)
        {
            _userrepo = urepository;
            _catrepo = crepository;
            this.log = log;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _imgRepo = imgRepo;
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

        public async Task<(object response, string message)> FilterImgByCategory(CategoryModel model)
        {
            if (model.catId < 1) return (response: null, message: "Failed. Check details passed.");
            var filtered = await _imgRepo.LoadWhere(c => c.CategoryId == model.catId);
            if (filtered == null || filtered.Count == 0) return (response: null, message: $"Failed. Couldn't find Images.");

            return (response: filtered, message: $"Success. Retrieved Images by this category.");
        }

        /// <summary>
        /// This method will get called to get all categories.
        /// </summary>
        /// <returns>
        /// Returns a tuple of the method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> GetCategories()
        {
            //Get categories from  db
            var cats = await _catrepo.LoadAll();
            if (cats == null) return (response: null, message: $"Couldnt retrieve images.");
            return (response: cats, message: $"Retrieved all Categories, {cats.Count} of them.");
        }

        /// <summary>
        /// This method will get called to get a category.
        /// </summary>
        /// <returns>
        /// Returns a tuple of the method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> GetCategory(int id)
        {
            if (id < 0) return (response: null, message: $"In correct parameter passed.");

            // try to get the image
            var cat = await _catrepo.FirstOrDefault(i => i.ID == id);
            if (cat == null) return (response: null, message: $"Category doesn't exist.");
            var categ = cat;
            return (response: categ, message: $"Success. Retrieved category {categ.CategoryName}");
        }
    }
}

/*
public async Task<(object response, string message)> GetImage(int id)
        {
            if(id > 0) return (response:null, message:$"In correct parameter passed.");
           
            // try to get the image
            var img = await _imgrepo.FirstOrDefault(i => i.ID ==id);
            if(img == null) return (response:null, message:$"Image doesn't exist.");
            var image = new ImageModel().ReturnImgModel(img);
            return (response:image, message:$"In correct parameter passed.");
        }

        /// <summary>
        /// This method will get called to get all images.
        /// </summary>
        /// <returns>
        /// Returns a tuple of the method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> RetrieveImages()
        {
            //Get from cloudinary | db
           var allImages = await _imgrepo.LoadAll();
           if(allImages==null) return (response:null, message:$"Couldnt retrieve images.");
           return (response:allImages, message:$"Retrieved all Images, {allImages.Count} of them.");
        }
*/