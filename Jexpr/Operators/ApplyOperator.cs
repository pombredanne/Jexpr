using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr.Operators
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ApplyOperator
    {
        [EnumMember(Value = "Exact")]
        Exact,

        [EnumMember(Value = "Percent")]
        Percent
    }
}