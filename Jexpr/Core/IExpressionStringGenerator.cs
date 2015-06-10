using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IExpressionStringGenerator
    {
        string GenerateFrom(BasicExpression expression);
    }
}