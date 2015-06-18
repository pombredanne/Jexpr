namespace Jexpr.Filters
{
    public class MinFilter : JexprFilter
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