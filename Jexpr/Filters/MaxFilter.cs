namespace Jexpr.Filters
{
    public class MaxFilter : AbstractFilter
    {
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder()
                                                .max()
                                                .value() )",
                parameterToChain, PropertyToVisit);

            return result;
        }
    }
}