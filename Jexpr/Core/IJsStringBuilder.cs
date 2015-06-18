using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IJsStringBuilder
    {
        string BuildFrom(JexprExpression expression);
    }
}