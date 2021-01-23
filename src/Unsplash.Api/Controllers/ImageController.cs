using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unsplash.Core.Services.Interfaces;
using Unsplash.Core.Models;
using Unsplash.Core.Util;
using Unsplash.Core.ApiModels;

namespace Unsplash.Api.Controllers
{
    public class ImageController : ControllerBase
    {
        private readonly IImageService _iserve;
        public ImageController(IImageService iserve)
        {
            _iserve = iserve;
        }

        #region  Image Endpoints

        /// <summary>
        /// Endpoints to Upload image to cloudinary.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImgToCloudinary(ImageModel model)
        {
            var response = new APIResponse();
            var (entity, message) = await _iserve.UploadImageCloudinary(model);
            if(entity != null)
            {
                response.Result = entity;
                response.ApiMessage = message;
                response.StatusCode = "00";
                return Ok(response);
            }
            response.ApiMessage = message;
            response.Result = entity;

            return BadRequest(response);
        }


        #endregion
    }
}