using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IJexprEngine
    {
        JexprResult<T> Evaluate<T>(ExpressionMetadata metadata, Dictionary<string, object> paramerters = null);
        JexprResult<T> Evaluate<T>(string script, Dictionary<string, object> paramerters = null);
    }
}