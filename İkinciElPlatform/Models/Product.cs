using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IkinciElPlatform.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }   // Ürün Adı

        [Required]
        public string Description { get; set; }  // Açıklama

        [Required]
        public decimal Price { get; set; }  // ✅ FİYAT ARTIK DOĞRU TİPTE

        public string? Brand { get; set; }

        public string? Condition { get; set; }

        public string? Location { get; set; }

        public bool IsNegotiable { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? SellerName { get; set; }

        public bool IsActive { get; set; } = true;

        // ✅ KATEGORİ BAĞLANTISI
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // ✅ KULLANICI BAĞLANTISI
        public string? UserId { get; set; }
    }
}
