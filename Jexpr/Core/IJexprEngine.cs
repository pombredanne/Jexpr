using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IJexprEngine
    {
        EvalResult<T> Evaluate<T>(ExpressionMetadata metadata, Dictionary<string, object> paramerters = null);
    }
}