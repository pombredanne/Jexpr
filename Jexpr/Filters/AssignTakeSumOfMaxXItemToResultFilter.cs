namespace Jexpr.Filters
{
    public class AssignTakeSumOfMaxXItemToResultFilter : AssignTakeToResultFilter, IHasResultProperty
    {
        public AssignTakeSumOfMaxXItemToResultFilter()
        {
            Take = 1;
            SortByOrder = true;
        }
    }
}