using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unsplash.Core.ApiModels;
using Unsplash.Core.DataAccess;
using Unsplash.Core.Models;
using Unsplash.Core.Util;
using Unsplash.Core.Services.Interfaces;

namespace Unsplash.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IRepository<User> _userrepo;
        private readonly IRepository<Category> _catrepo;
        private readonly IRepository<Photo> _imgrepo;
        private readonly ILogger<ImageService> log;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IRepository<User> urepository, IRepository<Category> crepository, ILogger<ImageService> log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IRepository<Photo> imgrepo)
        {
            _userrepo = urepository;
            _catrepo = crepository;
            this.log = log;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _imgrepo = imgrepo;
        }

        public Task<(object response, string message)> GetImage()
        {
            throw new System.NotImplementedException();
        }

        public Task<(object response, string message)> RetrieveImages()
        {
            throw new System.NotImplementedException();
        }

        public Task<(object response, string message)> SearchImageByName()
        {
            throw new System.NotImplementedException();
        }

        public Task<(object response, string message)> SearchImageByTagline()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This method will get called to upload image to cloudinary
        /// and also store information abotu the image to DB.
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns>
        /// Returns a tuple of thr method response and a descriptive message.
        /// </returns>
        public async Task<(object response, string message)> UploadImageCloudinary(ImageModel imageModel)
        {
            if (imageModel == null) return (response:null, message: $"Image upload Failed, check passed params.");

            //Cloud Acct
            var apiKey = configuration["CloudinaryData:ApiKey"];
            var apiSecret = configuration["CloudinaryData:ApiSecret"];
            var cloudAlias = configuration["CloudinaryData:CloudAlias"];


            //Call upload method to push to Cloudinary
            var (url, img, msg) = ProcessUpload.UploadToTheCloud(imageModel, apiKey, apiSecret, cloudAlias);
            if(img == null || url==null) return (response:null, message:msg);

            // save url to db 
            var newPhoto = new ImageModel().CreatePhoto(imageModel);
            newPhoto.FilePath = url;
            await _imgrepo.Insert(newPhoto);
            
            return (response: newPhoto, message:msg);
        }
    }
}