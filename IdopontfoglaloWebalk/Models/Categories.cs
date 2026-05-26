using System.ComponentModel.DataAnnotations;

namespace IdopontfoglaloWebalk.Models
{
    public class Categories
    {
        [Key]
        public int category_id {  get; set; }
        [Required]
        public string? category_name { get; set; }
        public virtual List<Services> Services { get; set; } = new();
    }
}
