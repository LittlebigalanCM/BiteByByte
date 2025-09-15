using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.Core.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Menu Item")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Range(0.01, 100.00, ErrorMessage = "Price must be between 0.01 and 100.00")]
        public float Price { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Food Type")]
        public int FoodTypeId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        [ForeignKey("FoodTypeId")]
        public virtual FoodType? FoodType { get; set; }
    }
}
