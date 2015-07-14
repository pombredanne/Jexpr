using System;
using Jexpr.Common;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AssignConditionalExactFilter : AbstractFilter, IHasResultProperty
    {
        public decimal ExactValue { get; private set; }

        public AssignConditionalExactFilter(decimal exactValue, string resultProperty, string propertyToVisit = "")
            : base(propertyToVisit)
        {
            ExactValue = exactValue;
            ResultProperty = resultProperty;
        }

        public override string ToJs(string parameterToChain)
        {
            var result = String.Format(@"( (function(){{
                                                if({0} !== undefined && {0}.length > 0){{
                                                   return {1};
                                                }} else {{
                                                   return 0;
                                                }}
                                            }})() )", parameterToChain, ExactValue.ToFormatted());

            return result;
        }

        public string ResultProperty { get; internal protected set; }
    }
}