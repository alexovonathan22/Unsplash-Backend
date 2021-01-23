using System.ComponentModel.DataAnnotations;

namespace Unsplash.Core.ApiModels
{
    public class CategoryModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CatName { get; set; }
    }
}