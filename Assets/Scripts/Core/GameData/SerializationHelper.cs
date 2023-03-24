using System.Text;
using Newtonsoft.Json;

namespace Core.GameData
{
internal static class SerializationHelper
{
    public static T DeserializeFromJson<T>(byte[] dataBytes)
    {
        var jsonString = Encoding.UTF8.GetString(dataBytes);
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static string SerializeToJson<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }
}
}