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
    public class CategoryController : ControllerBase
    {
         private readonly ICategoryService _catserv;
        private readonly ILogger<CategoryController> _log;


        public CategoryController(ICategoryService auth, ILogger<CategoryController> log)
        {
            _catserv = auth;
            _log = log;
        }
       #region Category Endpoints 
       /// <summary>
       /// Category Endpoints to filter image by category selected(passed)
       /// </summary>
       /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FilterImageByCategory(CategoryModel model)
        {
            var response = new APIResponse();
            var (entity, message) = await _catserv.FilterCategory(model);
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
       /// Category Endpoints to get all categories
       /// </summary>
       /// <returns></returns>
        [HttpGet("allcategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = new APIResponse();
            var (entity, message) = await _catserv.GetCategories();
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
       /// Category Endpoints to get a category
       /// </summary>
       /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetACategory(int id)
        {
            var response = new APIResponse();
            var (entity, message) = await _catserv.GetCategory(id);
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
       /// Category Endpoints to get a category
       /// </summary>
       /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryModel id)
        {
            var response = new APIResponse();
            var (entity, message) = await _catserv.CreateCAtegory(id);
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