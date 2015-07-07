using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr.Operators
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationOperator
    {
        [EnumMember(Value = "&&")]
        And,

        [EnumMember(Value = "||")]
        Or,

        [EnumMember(Value = "None")]
        None
    }
}