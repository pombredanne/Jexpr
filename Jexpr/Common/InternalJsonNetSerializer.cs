using Newtonsoft.Json;

namespace Jexpr.Common
{
    internal sealed class InternalJsonNetSerializer : IInternalSerializer
    {
        public T Deserialize<T>(string json, JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T value, JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                return JsonConvert.SerializeObject(value, settings);
            }

            return JsonConvert.SerializeObject(value);
        }
    }

}