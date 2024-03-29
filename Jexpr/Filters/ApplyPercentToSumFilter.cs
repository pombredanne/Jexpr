﻿using Jexpr.Interface;

namespace Jexpr.Filters
{
    public class ApplyPercentToSumFilter : SumFilter, IHasResultProperty
    {
        public double Percent { get; set; }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            var sumExpression = base.ToJs(parameterToChain);

            var result = string.Format(@"( ({0}) * ({1} / 100) )", sumExpression, Percent);

            return result;
        }
    }
}