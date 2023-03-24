using System.Collections.Generic;

namespace Core.GameConfigs
{
internal interface IReadOnlyConfigCollection<T> : IReadOnlyDictionary<string, T> where T : IGameConfig
{ }
}