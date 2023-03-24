using System;
using Newtonsoft.Json;

namespace Core.GameData
{
internal class BusinessData : BaseKeyGameData
{
    [JsonProperty("lvl")]
    public int Lvl;
    [JsonProperty("progress")]
    public double Progress;
    [JsonProperty("upgrade_ids")]
    public string[] UpgradeIds;

    public static BusinessData BuildDefault(string id)
    {
        return new BusinessData
        {
            Id = id,
            Lvl = 0,
            Progress = 0,
            UpgradeIds = Array.Empty<string>(),
        };
    }
}
}