using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SumOfXItemFilter : AbstractFilter
    {
        public SortDirection Direction { get; private set; }
        public int Take { get; private set; }

        public SumOfXItemFilter(string propertyToVisit, SortDirection sortDirection, int take = 0)
            : base(propertyToVisit)
        {
            Direction = sortDirection;
            Take = take;
        }

        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder({2})
                                                .take({3})
                                                .sum()
                                                .value() )",
                parameterToChain, PropertyToVisit,
                Direction == SortDirection.Ascending ? bool.TrueString.ToLower() : bool.FalseString.ToLower(), Take);

            return result;
        }
    }
}