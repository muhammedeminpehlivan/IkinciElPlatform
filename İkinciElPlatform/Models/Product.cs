using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        public string Location { get; set; }

        public bool IsNegotiable { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        // ❌ FORMDA YOK → VALIDATION DIŞI
        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public string SellerName { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public ICollection<Purchase> Purchases { get; set; }
    }
}
