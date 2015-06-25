using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExpressionGroupOp
    {
        [EnumMember(Value = "&&")]
        And,

        [EnumMember(Value = "||")]
        Or,

        [EnumMember(Value = "return")]
        Return
    }
}