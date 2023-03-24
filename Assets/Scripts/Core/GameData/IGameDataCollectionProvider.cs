namespace Core.GameData
{
internal interface IGameDataCollectionProvider<T> : IGameDataProvider<IReadOnlyDataCollection<T>>
    where T : BaseKeyGameData
{ }
}