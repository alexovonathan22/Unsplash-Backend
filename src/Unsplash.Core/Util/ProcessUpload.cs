using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Unsplash.Core.ApiModels;

namespace Unsplash.Core.Util
{
    public static class ProcessUpload
    {
        public static async Task<(string url, ImageUploadResult img, string message)> UploadToTheCloud(ImageModel model, string apiKey, string apiSecret, string cloudAlias, IWebHostEnvironment env)
        {
            // Enter account details
            var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = cloudAlias };
            //Create cloudinary object
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            string filePath = "";
            //Get image path
            if (CheckImageFile(model.ImgData))
            {
                filePath = await GetFilePathAsync(model.ImgData, env);
            }
            if(string.IsNullOrEmpty(filePath)) return (url:null, img:null, message:$"Failted to get correct file name.");
             
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(@$"{filePath}"),
                };

                var uploadResult =  _cloudinary.Upload(uploadParams);
                var returnurl = uploadResult.SecureUrl.AbsoluteUri;

                return (url:returnurl, img:uploadResult, message:$"Upload to cloudinary Successful.");
            }
            catch (Exception e)
            {
                return (url:null, img:null, message:$"Failed.Upload to cloudinary. \n {e.Message}.");
            }
            //
        }

        // PreUpload actions
         /// <summary>
        /// Method to check if file is image file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool CheckImageFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".jpg" || extension == ".png" ||  extension == ".jpeg"); 
        }

        private static async Task<string> GetFilePathAsync(IFormFile file, IWebHostEnvironment env)
        {
            string fileName;
            string path;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "cloudinary");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                path = Path.Combine(Directory.GetCurrentDirectory(), "cloudinary",
                   fileName);
                using (var stream = new FileStream(path, FileMode.Create))  
                {  
                    await file.CopyToAsync(stream);  
                }  
                return path;
            }
            catch (Exception e)
            {
               System.Console.WriteLine(e.Message);
               return null;
            }
        }
    }
}