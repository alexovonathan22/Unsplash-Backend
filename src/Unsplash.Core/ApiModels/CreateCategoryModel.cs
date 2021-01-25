using System;
using Unsplash.Core.Models;

namespace Unsplash.Core.ApiModels
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        internal Category PopulateCat(CreateCategoryModel model)
        {
            if(model==null) return null;

            var cat = new Category();
            cat.CategoryName = model.Name.ToLowerInvariant();
            cat.CreatedAt = DateTime.Now;
            cat.Description = model.Description;
            cat.ModifiedAt=DateTime.Now;

            return cat;
        }
    }
}