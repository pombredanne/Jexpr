using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jexpr
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FilterOperator
    {
        [EnumMember(Value = "None")]
        None,

        [EnumMember(Value = "SumOfMinXItem")]
        SumOfMinXItem,

        [EnumMember(Value = "SumOfMaxXItem")]
        SumOfMaxXItem,

        [EnumMember(Value = "Min")]
        Min,

        [EnumMember(Value = "Max")]
        Max,

        [EnumMember(Value = "Contains")]
        Contains,

        [EnumMember(Value = "GroupBy")]
        GroupBy,

        [EnumMember(Value = "Where")]
        Where,

        [EnumMember(Value = "Sum")]
        Sum,

        [EnumMember(Value = "Select")]
        Select,

        [EnumMember(Value = "Assign")]
        Assign
    }
}