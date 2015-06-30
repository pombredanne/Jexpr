using Newtonsoft.Json;

namespace Jexpr.Common
{
    internal interface IInternalSerializer
    {
        T Deserialize<T>(string json, JsonSerializerSettings settings = null);
        string Serialize<T>(T value, JsonSerializerSettings settings = null);
    }
}