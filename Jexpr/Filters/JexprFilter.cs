namespace Jexpr.Filters
{
    public abstract class JexprFilter
    {
        public MacroOp Op { get; set; }
        public string PropertyToVisit { get; set; }

        public abstract string ToJs(string parameterToChain);
    }
}