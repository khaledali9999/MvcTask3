using System.ComponentModel.DataAnnotations;

namespace MvcTask3.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? description { get; set; }
        public bool Status { get; set; }

    }
}
