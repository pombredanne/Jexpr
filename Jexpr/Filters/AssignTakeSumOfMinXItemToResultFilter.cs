using Jexpr.Interface;

namespace Jexpr.Filters
{
    public class AssignTakeSumOfMinXItemToResultFilter : AssignTakeToResultFilter, IHasResultProperty
    {
        public AssignTakeSumOfMinXItemToResultFilter()
        {
            Take = 1;
            SortByOrder = false;
        }
    }
}