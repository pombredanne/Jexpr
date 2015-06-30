namespace Jexpr.Filters
{
    public class GroupByFilter : AbstractFilter
    {
        public string Key { get; set; }
        public string GroupSet { get; set; }
        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                    .groupBy(function(item){{
                                                          var parts= '{1}'.split('.')
                                                          return item[parts[0]][parts[1]]        
                                                    }})
                                                    .pairs()
                                                    .map(function (currentItem) {{
                                                        return _.object(_.zip(['{2}', '{3}'], currentItem))
                                                    }})
                                                    .value()
                                                )", parameterToChain, PropertyToVisit, Key, GroupSet);

            return result;
        }
    }
}