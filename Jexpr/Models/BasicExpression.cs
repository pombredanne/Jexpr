namespace Jexpr.Models
{
    public class BasicExpression
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public ExpressionOp Operator { get; set; }
    }
}