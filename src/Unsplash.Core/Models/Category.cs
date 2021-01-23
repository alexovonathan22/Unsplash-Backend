using System.Collections.Generic;

namespace Unsplash.Core.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description{get;set;}
        public virtual List<Photo> Photos { get; set; }
    }
}