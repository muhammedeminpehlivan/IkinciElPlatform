using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IkinciElPlatform.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }   // ✅ SADECE DECIMAL

        public string? ImageUrl { get; set; }

        public string Brand { get; set; }
        public string Condition { get; set; }
        public string Location { get; set; }
        public bool IsNegotiable { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }

        public string SellerName { get; set; }
        public string UserId { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
