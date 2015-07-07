using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SumOfXItemFilter : AbstractFilter
    {
        private readonly SortDirection _sortDirection;
        private readonly int _take;

        public SumOfXItemFilter(string propertyToVisit, SortDirection sortDirection, int take = 0)
            : base(propertyToVisit)
        {
            _sortDirection = sortDirection;
            _take = take;
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
                _sortDirection == SortDirection.Ascending ? bool.TrueString.ToLower() : bool.FalseString.ToLower(), _take);

            return result;
        }
    }
}