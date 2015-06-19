namespace Jexpr.Models
{
    public class JexprResult<T>
    {
        public T Value { get; set; }

        public string Js { get; set; }
    }
}