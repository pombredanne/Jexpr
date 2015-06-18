using System;
using System.Collections.Generic;
using System.Text;

namespace Jexpr.Filters
{
    public class SetResultFilter : JexprFilter
    {
        public List<ResultProperty> ResultProperties { get; set; }
        public JexprFilter InnerFilter { get; set; }
        public override string ToJs(string parameterToChain)
        {

            StringBuilder builder = new StringBuilder();

            foreach (ResultProperty resultProperty in ResultProperties)
            {
                if (!string.IsNullOrEmpty(resultProperty.PropertyToPickUpFromParameters))
                {
                    builder.AppendFormat("result.{0} = p.{1} {2}", resultProperty.Name, resultProperty.PropertyToPickUpFromParameters, Environment.NewLine);
                }
                else
                {
                    IHasResultProperty hasResultProperty = InnerFilter as IHasResultProperty;

                    if (hasResultProperty != null)
                    {
                        if (resultProperty.Name == hasResultProperty.ResultProperty)
                        {
                            builder.AppendFormat("result.{0} =  {1} {2}", hasResultProperty.ResultProperty, InnerFilter.ToJs(parameterToChain), Environment.NewLine);
                        }
                    }
                }
            }

            return builder.ToString();
        }
    }
}