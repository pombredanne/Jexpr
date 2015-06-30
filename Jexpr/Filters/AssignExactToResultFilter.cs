using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    public class AssignExactToResultFilter : ReturnExactFilter, IHasResultProperty
    {
        public override string ToJs(string parameterToChain)
        {
            var baseJs = base.ToJs(parameterToChain);

            var result = String.Format(@"result.{0} = {1} ", ResultProperty, baseJs);

            return result;
        }
    }
}