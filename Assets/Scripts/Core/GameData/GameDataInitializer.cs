using Core.GameConfigs;

namespace Core.GameData
{
internal class GameDataInitializer : IGameDataInitializer
{
    private readonly GameConfigsProvider _gameConfigsProvider;

    public GameDataInitializer(GameConfigsProvider gameConfigsProvider)
    {
        _gameConfigsProvider = gameConfigsProvider;
    }

    public void InitGameData(ref GameData gameData)
    {
        gameData = new GameData();
        InitPlayerData(ref gameData);
        InitBusinessesData(ref gameData, _gameConfigsProvider);
    }

    private static void InitPlayerData(ref GameData gameData)
    {
        gameData.PlayerData.Balance = 0;
    }

    private static void InitBusinessesData(ref GameData gameData,
        IGameConfigCollectionsProvider businessesConfigsProvider)
    {
        foreach (var businessConfig in businessesConfigsProvider.BusinessConfigs.Values)
        {
            var businessData = BusinessData.BuildDefault(businessConfig.Id);
            if (businessConfig.IsInitial)
            {
                businessData.Lvl = 1;
            }

            gameData.BusinessesData.Add(businessConfig.Id, businessData);
        }
    }
}
}