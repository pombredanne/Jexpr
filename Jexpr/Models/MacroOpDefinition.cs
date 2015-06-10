namespace Jexpr.Models
{
    public class MacroOpDefinition
    {
        public virtual MacroOp Op { get; set; }
        public string PropertyToVisit { get; set; }

    }

    public class FilterMacroOpDefinition : MacroOpDefinition
    {
        public int Take { get; set; }
    }
}