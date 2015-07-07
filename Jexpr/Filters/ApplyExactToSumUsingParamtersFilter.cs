using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplyExactToSumUsingParamtersFilter : SumFilter, IHasResultProperty
    {
        private readonly string _parameterName;

        public ApplyExactToSumUsingParamtersFilter(string propertyToVisit, string parameterName, string resultProperty)
            : base(propertyToVisit)
        {
            _parameterName = parameterName;
            ResultProperty = resultProperty;
        }


        public string ResultProperty { get; internal protected set; }

        public override string ToJs(string parameterToChain)
        {
            var sumExpression = base.ToJs(parameterToChain);

            var result = String.Format(@"( ({0}) - (p.{1})", sumExpression, _parameterName);

            return result;
        }
    }
}