using System.Collections.Generic;

namespace Jexpr.Filters
{
    public abstract class AbstractFilter
    {
        public string Property { get; set; }
        public List<AbstractFilter> Conditions { get; set; }

        public abstract string ToJs(string parameterToChain);
    }
}