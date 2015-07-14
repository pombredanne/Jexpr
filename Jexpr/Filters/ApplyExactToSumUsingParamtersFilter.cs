using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplyExactToSumUsingParamtersFilter : SumFilter, IHasResultProperty
    {
        public string ParameterName { get; private set; }

        public ApplyExactToSumUsingParamtersFilter(string propertyToVisit, string parameterName, string resultProperty)
            : base(propertyToVisit)
        {
            ParameterName = parameterName;
            ResultProperty = resultProperty;
        }


        public string ResultProperty { get; internal protected set; }

        public override string ToJs(string parameterToChain)
        {
            var sumExpression = base.ToJs(parameterToChain);

            var result = String.Format(@"( ({0}) - (p.{1})", sumExpression, ParameterName);

            return result;
        }
    }
}