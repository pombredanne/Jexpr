namespace Jexpr.Filters
{
    public abstract class AbstractFilter
    {
        public FilterOperator Operator { get; set; }
        public string PropertyToVisit { get; set; }

        public abstract string ToJs(string parameterToChain);
    }
}