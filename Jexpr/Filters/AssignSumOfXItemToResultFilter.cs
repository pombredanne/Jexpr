using System;
using Jexpr.Interface;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AssignSumOfXItemToResultFilter : SumOfXItemFilter, IHasResultProperty
    {
        public AssignSumOfXItemToResultFilter(string propertyToVisit, SortDirection sortDirection, int take, string resultProperty)
            : base(propertyToVisit, sortDirection, take)
        {
            ResultProperty = resultProperty;
        }

        public string ResultProperty { get; internal protected set; }

        public override string ToJs(string parameterToChain)
        {
            string sumExpression = base.ToJs(parameterToChain);

            string result = String.Format(@"( {0} )", sumExpression);

            return result;
        }
    }
}