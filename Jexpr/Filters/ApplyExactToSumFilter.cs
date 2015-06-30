using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    public class ApplyExactToSumFilter : SumFilter, IHasResultProperty
    {
        public decimal Amount { get; set; }

        //TODO: Operator (+ - / *)
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            var sumExpression = base.ToJs(parameterToChain);

            var result = String.Format(@"( ({0}) - ({1})", sumExpression, Amount);

            return result;
        }
    }
}