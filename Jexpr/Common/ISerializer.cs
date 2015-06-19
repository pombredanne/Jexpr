namespace Jexpr.Common
{
    internal interface ISerializer
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T value);
    }
}