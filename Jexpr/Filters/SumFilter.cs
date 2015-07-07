namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SumFilter : AbstractFilter
    {
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sum()
                                                .value() )",
                parameterToChain, PropertyToVisit);

            return result;
        }
    }
}