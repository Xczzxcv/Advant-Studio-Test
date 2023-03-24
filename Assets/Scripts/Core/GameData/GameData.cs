using System;
using Newtonsoft.Json;

namespace Core.GameData
{
[Serializable]
internal class GameData
{
    [JsonProperty("player_data")]
    public readonly PlayerData PlayerData = new();
    [JsonProperty("businesses_data")]
    public readonly GameDataCollection<BusinessData> BusinessesData = new();
}

[Serializable]
internal class PlayerData : IGameData
{
    [JsonProperty("balance")]
    public double Balance;
}
}