using Newtonsoft.Json;

namespace Jexpr.Filters
{
    public class IndexOfFilter : AbstractFilter
    {
        public object ValueToLookup { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string parameter = !string.IsNullOrEmpty(parameterToChain) && (parameterToChain != "p.") ? parameterToChain : string.Format("p.{0}", PropertyToVisit);

            string result = string.Format(@"( {0}.indexOf({1}) !== -1 )", JsonConvert.SerializeObject(ValueToLookup), parameter);

            return result;
        }
    }
}