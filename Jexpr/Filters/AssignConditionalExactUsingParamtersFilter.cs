using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AssignConditionalExactUsingParamtersFilter : AbstractFilter, IHasResultProperty
    {
        public string ParameterName { get; private set; }

        public AssignConditionalExactUsingParamtersFilter(string parameterName, string resultProperty, string propertyToVisit = "")
            : base(propertyToVisit)
        {
            ParameterName = parameterName;
            ResultProperty = resultProperty;
        }

        public string ResultProperty { get; protected internal set; }

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