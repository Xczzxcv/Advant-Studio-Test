using Newtonsoft.Json;

namespace Core.GameData
{
internal class BusinessUpgradeData : BaseKeyGameData
{
    [JsonProperty("id")]
    public string Id { get; }
}
}