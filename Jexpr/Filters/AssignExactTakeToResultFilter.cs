﻿using System;
using Jexpr.Interface;

namespace Jexpr.Filters
{
    public class AssignExactTakeToResultFilter : SumOfXItemFilter, IHasResultProperty
    {
        public AssignExactTakeToResultFilter()
        {
            Take = 1;
        }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string sumExpression = base.ToJs(parameterToChain);

            string result = String.Format(@"( (function(){{ return {0} }})() )", sumExpression);

            return result;
        }
    }
}