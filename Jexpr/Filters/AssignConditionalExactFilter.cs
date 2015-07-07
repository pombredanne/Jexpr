using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AssignConditionalExactFilter : AbstractFilter, IHasResultProperty
    {
        private readonly decimal _exactValue;

        public AssignConditionalExactFilter(decimal exactValue, string resultProperty, string propertyToVisit = "") : base(propertyToVisit)
        {
            _exactValue = exactValue;
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
                                            }})() )", parameterToChain, _exactValue);

            return result;
        }

        public string ResultProperty { get; internal protected set; }
    }
}