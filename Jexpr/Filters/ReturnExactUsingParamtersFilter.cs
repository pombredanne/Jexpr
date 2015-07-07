using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    public class ReturnExactUsingParamtersFilter : AbstractFilter, IHasResultProperty
    {
        public string ParameterName { get; set; }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            var result = String.Format(@"( (function(){{
                                                if({0} !== undefined && {0}.length > 0){{
                                                    return p.{1};
                                                }} else {{ 
                                                    return 0; 
                                                }}
                                            }})() )", parameterToChain, ParameterName);

            return result;
        }
    }
}