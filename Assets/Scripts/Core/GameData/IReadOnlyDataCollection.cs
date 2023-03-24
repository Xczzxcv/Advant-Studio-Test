using System.Collections.Generic;

namespace Core.GameData
{
internal interface IReadOnlyDataCollection<T> : IReadOnlyDictionary<string, T>, IGameData
{ }
}