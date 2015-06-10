namespace Jexpr.Models
{
    public class JexprExpression
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public ExpressionOp Operator { get; set; }
    }
}