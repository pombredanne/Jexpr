using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExpressionOp
    {
        [EnumMember(Value = "===")]
        Equal,

        [EnumMember(Value = "!==")]
        NotEqual,

        [EnumMember(Value = ">")]
        GreaterThan,

        [EnumMember(Value = "<")]
        LowerThan,

        [EnumMember(Value = ">=")]
        GreaterThenOrEqual,

        [EnumMember(Value = "<=")]
        LowerThanEqual
    }
}