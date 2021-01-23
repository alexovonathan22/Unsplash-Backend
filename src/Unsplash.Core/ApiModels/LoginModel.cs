using System.ComponentModel.DataAnnotations;

namespace Unsplash.Core.ApiModels
{
   public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}