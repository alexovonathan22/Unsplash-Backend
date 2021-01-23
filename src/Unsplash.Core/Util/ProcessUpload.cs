using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Unsplash.Core.ApiModels;

namespace Unsplash.Core.Util
{
    public static class ProcessUpload
    {
        public static (string url, ImageUploadResult img, string message) UploadToTheCloud(ImageModel model, string apiKey, string apiSecret, string cloudAlias)
        {
            // Enter account details
            var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = cloudAlias };
            //Create cloudinary object
            Cloudinary _cloudinary = new Cloudinary(myAccount);

            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(model.FilePath),
                };

                var uploadResult =  _cloudinary.Upload(uploadParams);
                var returnurl = uploadResult.SecureUrl.AbsoluteUri;

                return (url:returnurl, img:uploadResult, message:$"Upload to cloudinary Successful.");
            }
            catch (Exception e)
            {
                return (url:null, img:null, message:$"Failed.Upload to cloudinary \n {e.Message}.");
            }
            //
        }
    }
}