using System.Collections.Generic;

namespace Jexpr.Tests
{
    public class PromotionApplyResult
    {
        public decimal Discount { get; set; }
        public int PromotionId{ get; set; }
        public Basket Basket{ get; set; }
    }
}