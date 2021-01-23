using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unsplash.Core.Models
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public DateTime? CreatedAt { get; set; }
        // Set current time 
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedAt { get; set; }

    }
}