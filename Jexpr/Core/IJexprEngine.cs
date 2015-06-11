using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IJexprEngine
    {
        EvalResult Evaluate(ExpressionDefinition expression, Dictionary<string, object> paramerters = null);
    }
}