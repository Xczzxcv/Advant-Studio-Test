namespace Core.GameData
{
internal interface IGameDataProvider<T> where T : IGameData
{
    T GetData();
}
}