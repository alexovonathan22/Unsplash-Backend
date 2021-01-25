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
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> UploadImgToCloudinary([FromForm]ImageModel model)
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

        /// <summary>
        /// Endpoints to get all uploaded images to cloudinary.
        /// </summary>
        /// <returns></returns>
        [HttpGet("viewall")]
        public async Task<IActionResult> RetrieveImages()
        {
            var response = new APIResponse();
            var (entity, message) = await _iserve.RetrieveImages();
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

        [HttpGet("{imageid}")]
        public async Task<IActionResult> RetrieveImage(int imageid)
        {
            var response = new APIResponse();
            var (entity, message) = await _iserve.GetImage(imageid);
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


        [HttpGet("tags/{tagline}")]
        public async Task<IActionResult> SearchForImageByTag(string tagline)
        {
            var response = new APIResponse();
            var (entity, message) = await _iserve.SearchImageByTagline(tagline);
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

        [HttpGet("name/{name}")]
        public async Task<IActionResult> SearchForImageByName(string name)
        {
            var response = new APIResponse();
            var (entity, message) = await _iserve.SearchImageByName(name);
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