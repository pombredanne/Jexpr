using System.Collections.Generic;

namespace Jexpr.Tests.Models
{
    internal class PromotionRequest
    {
        public Dictionary<string, object> Parameters { get; set; }

        public Basket Basket { get; set; }
    }
}