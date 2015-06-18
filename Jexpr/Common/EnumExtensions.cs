using System;
using System.Runtime.Serialization;

namespace Jexpr.Common
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<EnumMemberAttribute>();
            return attribute == null ? value.ToString() : attribute.Value;
        }

    }
}