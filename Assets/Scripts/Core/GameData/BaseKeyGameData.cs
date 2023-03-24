using System;
using Newtonsoft.Json;

namespace Core.GameData
{
[Serializable]
internal abstract class BaseKeyGameData : IGameData
{
    [JsonProperty("id")]
    public string Id;
}
}