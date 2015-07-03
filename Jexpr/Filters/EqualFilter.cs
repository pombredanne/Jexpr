namespace Jexpr.Filters
{
    public class EqualFilter : AbstractFilter
    {
        public object Value { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( {0} === p.{1} )", Value.ToString().ToLower(), PropertyToVisit);

            return result;
        }
    }
}