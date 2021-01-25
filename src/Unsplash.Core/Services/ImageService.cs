using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unsplash.Core.ApiModels;
using Unsplash.Core.DataAccess;
using Unsplash.Core.Models;
using Unsplash.Core.Util;
using Unsplash.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Unsplash.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IRepository<User> _userrepo;
        private readonly IRepository<Category> _catrepo;
        private readonly IRepository<Photo> _imgrepo;
        private readonly ILogger<ImageService> log;
        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IRepository<User> urepository, IRepository<Category> crepository, ILogger<ImageService> log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IRepository<Photo> imgrepo, IWebHostEnvironment env)
        {
            _userrepo = urepository;
            _catrepo = crepository;
            this.log = log;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _imgrepo = imgrepo;
            _env = env;
        }

        public async Task<(object response, string message)> GetImage(int id)
        {
            if (id > 0) return (response: null, message: $"In correct parameter passed.");

            // try to get the image
            var img = await _imgrepo.FirstOrDefault(i => i.ID == id);
            if (img == null) return (response: null, message: $"Image doesn't exist.");
            var image = new ImageModel().ReturnImgModel(img);
            return (response: image, message: $"Success. Retrieved Image.");
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
            if (allImages == null) return (response: null, message: $"Couldnt retrieve images.");
            return (response: allImages, message: $"Retrieved all Images, {allImages.Count} of them.");
        }

        /// <summary>
        /// This method will get called to Search image to by Name
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns>
        /// Returns a tuple of the method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> SearchImageByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return (response: null, message: $"In correct parameter passed.");

            // try to get the image
            var img = await _imgrepo.FirstOrDefault(i => i.PhotoName == name);
            if (img == null) return (response: null, message: $"Image doesn't exist.");
            var image = new ImageModel().ReturnImgModel(img);
            return (response: image, message: $"Success. Retrieved image with {name}");
        }

        /// <summary>
        /// This method will get called to Search image to by Tagline
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns>
        /// Returns a tuple of the method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> SearchImageByTagline(string tagline)
        {
            //Do note that tags are either strings seperated by ; or,
            //tHey could be collected as arrays of string but are to be submitted as strings ,or; separated
            if (string.IsNullOrEmpty(tagline)) return (response: null, message: $"In correct parameter passed.");

            // try to get the image or images
            var img = await _imgrepo.LoadWhere(i => i.Tags.Contains(tagline));
            if (img == null) return (response: null, message: $"Image doesn't exist.");
            var image = img;
            return (response: image, message: $"Success. Retrieved image(s) with {tagline}");
        }

        /// <summary>
        /// This method will get called to upload image to cloudinary
        /// and also store information about the image to DB.
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns>
        /// Returns a tuple of the method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> UploadImageCloudinary(ImageModel imageModel)
        {
            if (imageModel == null) return (response: null, message: $"Image upload Failed, check passed params.");
            var userCtx = _httpContextAccessor.HttpContext.User.Identity.Name;
            var usr = await _userrepo.FirstOrDefault(t => t.Username == userCtx);
            //Cloud Acct
            var apiKey = configuration["CloudinaryData:ApiKey"];
            var apiSecret = configuration["CloudinaryData:ApiSecret"];
            var cloudAlias = configuration["CloudinaryData:CloudAlias"];
            // Category is stored in lower case
            var checkCat =await _catrepo.FirstOrDefault(c => c.CategoryName == imageModel.CategoryName.ToLowerInvariant());
            if (checkCat == null) return (response: null, message: $"{imageModel.CategoryName} doesnt exists");

            var imgExist = _imgrepo.FirstOrDefault(t => t.PhotoName == imageModel.PhotoName.ToLowerInvariant());
            //Call upload method to push to Cloudinary
            var (url, img, msg) = await ProcessUpload.UploadToTheCloud(imageModel, apiKey, apiSecret, cloudAlias, _env);
            if (img == null || url == null) return (response: null, message: msg);

            var tags = imageModel.Tags.ToLowerInvariant();
            // save url to db 
            var newPhoto = new ImageModel().CreatePhoto(imageModel);
            newPhoto.FilePath = url;
            newPhoto.UserId = usr.ID;
            newPhoto.CategoryId = checkCat.ID;
            newPhoto.CategoryName = checkCat.CategoryName;
            newPhoto.PhotoName = imageModel.PhotoName.ToLowerInvariant();
            newPhoto.Tags = tags;

            
            try
            {
                await _imgrepo.Insert(newPhoto);
            }
            catch (DbUpdateException ex)
            {
                log.LogError($"=> {ex}");
                return (response: null, message: $"{ex.Message}");
            }

            return (response: newPhoto, message: msg);
        }
    }
}