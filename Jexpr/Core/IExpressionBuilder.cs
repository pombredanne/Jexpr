using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Core
{
    public interface IExpressionBuilder
    {
        JsExpressionResult Build(ExpressionDefinition definition, Dictionary<string, object> paramerters);
    }
}