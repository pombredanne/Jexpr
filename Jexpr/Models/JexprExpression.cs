namespace Jexpr.Models
{
    public abstract class JexprExpression
    {
        protected JexprExpression()
        {
            HasPriority = false;
        }
        public string Key { get; set; }
        public object Value { get; set; }
        public bool HasPriority { get; set; }
    }
}