namespace Jexpr.Filters
{
    public abstract class AbstractFilter
    {
        public string PropertyToVisit { get; set; }

        public abstract string ToJs(string parameterToChain);
    }
}