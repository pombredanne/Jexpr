using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr.Operators
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConditionOperator
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
        GreaterThanOrEqual,

        [EnumMember(Value = "<=")]
        LowerThanEqual,

        [EnumMember(Value = "&&")]
        And,

        [EnumMember(Value = "||")]
        Or,

        [EnumMember(Value = "SubSet")]
        SubSet,
        
        [EnumMember(Value = "Contains")]
        Contains,

        [EnumMember(Value = "%")]
        Mod,
    }
}