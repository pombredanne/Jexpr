namespace Jexpr.Filters
{
    public class TakeFilter : JexprFilter
    {
        internal bool SortByOrder { get; set; }
        public int Take { get; set; }
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder({3})
                                                .take({2})
                                                .sum()
                                                .value() )",
                parameterToChain, PropertyToVisit, Take, SortByOrder);

            return result;
        }
    }
}