using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// Assign exact value to result
    /// </summary>
    public class AssignExactToResultFilter : AssignConditionalExactFilter, IHasResultProperty
    {
        public AssignExactToResultFilter(decimal exactValue, string resultProperty, string propertyToVisit = "")
            : base(exactValue, resultProperty, propertyToVisit)
        {
        }

        public override string ToJs(string parameterToChain)
        {
            var baseJs = base.ToJs(parameterToChain);

            var result = String.Format(@"result.{0} = {1} ", ResultProperty, baseJs);

            return result;
        }
    }
}