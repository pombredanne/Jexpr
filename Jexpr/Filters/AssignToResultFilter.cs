using System;
using System.Collections.Generic;
using System.Text;
using Jexpr.Interface;
using Jexpr.Models;

namespace Jexpr.Filters
{
    public class AssignToResultFilter : AbstractFilter
    {
        public List<ResultProperty> ResultSet { get; set; }
        public AbstractFilter Filter { get; set; }
        public override string ToJs(string parameterToChain)
        {

            StringBuilder builder = new StringBuilder();

            foreach (ResultProperty resultProperty in ResultSet)
            {
                if (!string.IsNullOrEmpty(resultProperty.PickUpFromParameters))
                {
                    builder.AppendFormat("result.{0} = p.{1} {2}", resultProperty.Name, resultProperty.PickUpFromParameters, Environment.NewLine);
                }
                else
                {
                    IHasResultProperty hasResultProperty = Filter as IHasResultProperty;

                    if (hasResultProperty != null)
                    {
                        if (resultProperty.Name == hasResultProperty.ResultProperty)
                        {
                            builder.AppendFormat("result.{0} =  {1} {2}", hasResultProperty.ResultProperty, Filter.ToJs(parameterToChain), Environment.NewLine);
                        }
                    }
                }
            }

            return builder.ToString();
        }
    }
}