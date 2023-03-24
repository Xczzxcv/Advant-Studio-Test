using System;
using System.Collections.Generic;

namespace Core.GameData
{
[Serializable]
internal class GameDataCollection<T> : Dictionary<string, T>, IReadOnlyDataCollection<T>
    where T : BaseKeyGameData
{ }
}