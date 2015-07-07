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
        public AssignSumOfXItemToResultFilter(SortDirection sortDirection, int take = 0)
            : base(sortDirection, take)
        {
        }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string sumExpression = base.ToJs(parameterToChain);

            string result = String.Format(@"( {0} )", sumExpression);

            return result;
        }
    }
}