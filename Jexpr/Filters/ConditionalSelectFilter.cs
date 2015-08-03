namespace Jexpr.Filters
{
    public class ConditionalSelectFilter : SelectFilter
    {
        public override string ToJs(string parameterToChain)
        {
            var baseJs = base.ToJs(parameterToChain);

            var result = string.Format(@"(function () {{
                                          {0}
                                          return {1}.length > 0;
                                      }})()", baseJs, parameterToChain);

            return result;
        }
    }
}