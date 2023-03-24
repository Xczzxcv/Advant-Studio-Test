using System.Collections.Generic;

namespace Core.GameConfigs
{
internal class GameConfigCollection<T> : Dictionary<string, T>, IReadOnlyConfigCollection<T>
    where T : IGameConfig
{ }
}