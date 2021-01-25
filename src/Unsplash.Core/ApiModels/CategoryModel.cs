using System.ComponentModel.DataAnnotations;

namespace Unsplash.Core.ApiModels
{
    public class CategoryModel
    {
        [Required]
        public int catId { get; set; }
        
    }
}