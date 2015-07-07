using System.Collections.Generic;

namespace Jexpr.Tests.Models
{
    internal class TestPromotionRequest
    {
        public Dictionary<string, object> Parameters { get; set; }

        public TestBasket TestBasket { get; set; }
    }
}