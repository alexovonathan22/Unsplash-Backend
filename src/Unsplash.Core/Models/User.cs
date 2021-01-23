using System.Collections.Generic;

namespace Unsplash.Core.Models
{
      public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual List<Photo> Photos { get; set; }
    }
}