namespace Jexpr.Models
{
    public class EvalResult<T>
    {
        public T Value { get; set; }

        public string Js { get; set; }
    }
}