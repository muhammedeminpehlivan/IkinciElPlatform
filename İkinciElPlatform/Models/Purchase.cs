using System;

namespace IkinciElPlatform.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string BuyerId { get; set; }
        public string SellerId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string Status { get; set; } // Satıldı
    }
}
