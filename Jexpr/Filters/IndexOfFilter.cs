using Newtonsoft.Json;

namespace Jexpr.Filters
{
    public class IndexOfFilter : JexprFilter
    {
        public object ValueToLookup { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( {0}.indexOf({1}) !== -1 )", JsonConvert.SerializeObject(ValueToLookup), parameterToChain);

            return result;
        }
    }
}