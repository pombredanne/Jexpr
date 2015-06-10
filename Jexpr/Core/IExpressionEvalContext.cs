using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IExpressionEvalContext
    {
        ExpressionEvalResult Evaluate(ExpressionDefinition expression, Dictionary<string, object> paramerters = null);
    }
}