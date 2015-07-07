using System.Collections.Generic;

namespace Jexpr.Tests.Models
{
    internal class TestProduct
    {
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}