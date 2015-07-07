namespace Jexpr.Filters
{
    public class SumWithMultiplyFilter : AbstractFilter
    {
        private readonly string _multiplier;

        public SumWithMultiplyFilter(string multiplier)
        {
            _multiplier = multiplier;
        }

        public override string ToJs(string parameterToChain)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(_multiplier))
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
                                    }})()", parameterToChain, Property, _multiplier);
            }

            return result;
        }
    }
}