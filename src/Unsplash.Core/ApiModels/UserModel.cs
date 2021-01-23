using System.ComponentModel.DataAnnotations;

namespace Unsplash.Core.ApiModels
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public int Id { get; set; }
    }
}