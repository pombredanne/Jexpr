﻿using Jexpr.Models;

namespace Jexpr.Core
{
    internal interface IJsStringBuilder
    {
        string BuildFrom(JexprExpression expression);
    }
}