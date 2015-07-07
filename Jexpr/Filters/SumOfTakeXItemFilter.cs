namespace Jexpr.Filters
{
    public class SumOfXItemFilter : AbstractFilter
    {
        internal bool SortByOrder { get; set; }
        public int Take { get; set; }
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder({2})
                                                .take({3})
                                                .sum()
                                                .value() )",
                parameterToChain, Property,
                SortByOrder ? bool.TrueString.ToLower() : bool.FalseString.ToLower(), Take);

            return result;
        }
    }
}