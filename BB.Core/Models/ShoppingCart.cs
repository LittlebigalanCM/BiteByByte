using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.Core.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Count { get; set; } = 1;
        public int MenuItemId { get; set; }
        public string? ApplicationUserId { get; set; }

        [NotMapped]
        public virtual MenuItem? MenuItem { get; set; }

        [NotMapped]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
