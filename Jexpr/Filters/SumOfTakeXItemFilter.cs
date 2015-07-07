using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SumOfXItemFilter : AbstractFilter
    {
        public SumOfXItemFilter(SortDirection sortDirection, int take = 0)
        {
            SortDirection = sortDirection;
            Take = take;
        }
        internal SortDirection SortDirection { get; set; }
        internal  protected int Take { get; set; }
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder({2})
                                                .take({3})
                                                .sum()
                                                .value() )",
                parameterToChain, PropertyToVisit,
                SortDirection == SortDirection.Ascending ? bool.TrueString.ToLower() : bool.FalseString.ToLower(), Take);

            return result;
        }
    }
}