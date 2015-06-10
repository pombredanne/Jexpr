using Newtonsoft.Json;

namespace Jexpr.Common
{
    internal class JsonNetSerializer : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}