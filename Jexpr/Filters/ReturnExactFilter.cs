using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// </summary>
    public class ReturnExactFilter : AbstractFilter, IHasResultProperty
    {
        public decimal Value { get; set; }
        public override string ToJs(string parameterToChain)
        {
            var result = String.Format(@"( (function(){{
                                                if({0} !== undefined && {0}.length > 0){{
                                                   return {1};
                                                }} else {{
                                                   return 0;
                                                }}
                                            }})() )", parameterToChain, Value);

            return result;
        }

        public string ResultProperty { get; set; }
    }
}