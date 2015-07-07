using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr.Operators
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FunctionOperator
    {
        [EnumMember(Value = "Min")]
        Min,

        [EnumMember(Value = "Max")]
        Max
    }
}