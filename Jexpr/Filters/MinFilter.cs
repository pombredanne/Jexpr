namespace Jexpr.Filters
{
    public class MinFilter : AbstractFilter
    {
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder(false)
                                                .min()
                                                .value() )", parameterToChain, PropertyToVisit);

            return result;
        }
    }
}