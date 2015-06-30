using System;

namespace Jexpr.Filters
{
    public class ApplyExactToSumUsingParamtersFilter : SumFilter, IHasResultProperty
    {
        public string ParameterName { get; set; }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            var sumExpression = base.ToJs(parameterToChain);

            var result = String.Format(@"( ({0}) - (p.{1})", sumExpression, ParameterName);

            return result;
        }
    }
}