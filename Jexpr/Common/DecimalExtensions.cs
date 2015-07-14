namespace Jexpr.Common
{
    public static class DecimalExtensions
    {
        public static string ToFormatted(this decimal value)
        {
            var format = string.Format("{0:0.00}", value);

            string formatted = format.EndsWith(".00") ? format.Substring(0, format.LastIndexOf(".00")) : format;

            return formatted.Replace(",", ".");
        }
    }
}