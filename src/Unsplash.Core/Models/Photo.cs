using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Unsplash.Core.Models
{
    [Table("Images")]
    public class Photo : BaseEntity
    {
        public string PhotoName { get; set; }
        public string FilePath { get; set; }
        public string Tags { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UserId {get;set;}

        [ForeignKey("CategoryId")]
        [JsonIgnore]
        public virtual Category GetCategory { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User GetUser { get; set; }
    }
}