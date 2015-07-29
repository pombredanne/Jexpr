using System.Collections.Generic;

namespace Jexpr.Tests.Models
{
    internal class Basket
    {
        public decimal TotalPrice { get; set; }

        public List<Product> Products { get; set; }
    }

    internal class ApplyPromotionRequest
    {
        public ApplyPromotionRequest()
        {
            Parameters = new Dictionary<string, object>();
        }
        public Dictionary<string, object> Parameters { get; set; }
        public string PromotionCode { get; set; }
        public ApplyMode ApplyMode { get; set; }
        public Basket Basket { get; set; }
    }

    internal class Product
    {
        public decimal TotalPrice { get { return Quantity * UnitPrice; } }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }

    internal enum ApplyMode
    {
        ReadOnly,
        Apply
    }
}