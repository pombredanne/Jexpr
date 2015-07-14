namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SumWithMultiplyFilter : AbstractFilter
    {
        public string Multiplier { get; private set; }

        public SumWithMultiplyFilter(string propertyToVisit, string multiplier)
            : base(propertyToVisit)
        {
            Multiplier = multiplier;
        }

        public override string ToJs(string parameterToChain)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(Multiplier))
            {
                result = string.Format(@"  
                                  (function () {{
                                        var _pTotal=0;
                                        _.chain({0}).each(function (item) {{
                                            var _p{1} = item.{1};
                                            var _p{2} = item.{2};

                                            _pTotal += (_p{1} * _p{2});
                                        }}).value();
            
                                        return _pTotal;
                                    }})()", parameterToChain, PropertyToVisit, Multiplier);
            }

            return result;
        }
    }
}