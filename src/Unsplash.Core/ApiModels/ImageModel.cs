using System;
using Unsplash.Core.Models;

namespace Unsplash.Core.ApiModels
{
    public class ImageModel
    {
        public string PhotoName { get; set; }
        public string FilePath { get; set; }
        public string Tags { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        internal Photo CreatePhoto(ImageModel imageModel)
        {
            if (imageModel == null) return null;

            var photo = new Photo();
            photo.CategoryId = imageModel.CategoryId;
            photo.CategoryName=imageModel.CategoryName;
            photo.CreatedAt =DateTime.Now;
            photo.ModifiedAt=DateTime.Now;

            return photo;
        }
        internal ImageModel ReturnImgModel(Photo imageModel)
        {
            if (imageModel == null) return null;

            var photo = new ImageModel();
            photo.CategoryId = imageModel.CategoryId;
            photo.CategoryName=imageModel.CategoryName;
            photo.FilePath =imageModel.FilePath;
            photo.PhotoName =imageModel.PhotoName;
            photo.Tags =imageModel.Tags;


            return photo;
        }

    }
}