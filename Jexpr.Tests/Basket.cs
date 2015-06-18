using System.Collections.Generic;

namespace Jexpr.Tests
{
    public class Product
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }

    public class Basket
    {
        public List<Product> Products { get; set; }
    }

    public class PromotionRequest
    {
        public Dictionary<string, object> Parameters { get; set; }

        public Basket Basket { get; set; }
    }
}