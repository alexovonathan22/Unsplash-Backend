using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Unsplash.Core.Models;

namespace Unsplash.Core.ApiModels
{
    public class ImageModel
    {
        public string PhotoName { get; set; }
        // NO need for file path IFormfile is here
        // public string FilePath { get; set; }
        public string Tags { get; set; }
        public string ImgPath { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please choose image")]
        [Display(Name = "Upload Image")] 
        public IFormFile ImgData { get; set; }

        internal Photo CreatePhoto(ImageModel imageModel)
        {
            if (imageModel == null) return null;

            var photo = new Photo();
            photo.CreatedAt = DateTime.Now;
            photo.ModifiedAt = DateTime.Now;

            return photo;
        }
        internal ImageModel ReturnImgModel(Photo imageModel)
        {
            if (imageModel == null) return null;

            var photo = new ImageModel();
            photo.CategoryName = imageModel.CategoryName;
            photo.PhotoName = imageModel.PhotoName;
            photo.Tags = imageModel.Tags;
            photo.ImgPath = imageModel.FilePath;

            return photo;
        }

    }
}