using System.Collections.Generic;

namespace Jexpr.Tests.Models
{
    internal class Product
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}