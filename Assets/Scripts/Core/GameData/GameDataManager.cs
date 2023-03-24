using System;
using Core.Ecs;

namespace Core.GameData
{
internal class GameDataManager :
    IGameDataCollectionProvider<BusinessData>,
    IPlayerDataProvider
{
    private readonly ISaveDataHandler _saveDataHandler;
    private readonly GameData _gameData;

    public GameDataManager(ISaveDataHandler saveDataHandler,
        IGameDataInitializer gameDataInitializer)
    {
        _saveDataHandler = saveDataHandler;
        if (!_saveDataHandler.IsSaveDataExists())
        {
            gameDataInitializer.InitGameData(ref _gameData);
            return;
        }

        if (!_saveDataHandler.TryGetSaveData(out _gameData))
        {
            throw new AggregateException("Can't get saved data");
        }
    }

    IReadOnlyDataCollection<BusinessData> IGameDataProvider<IReadOnlyDataCollection<BusinessData>>.GetData()
    {
        return _gameData.BusinessesData;
    }

    PlayerData IGameDataProvider<PlayerData>.GetData()
    {
        return _gameData.PlayerData;
    }

    public void UpdateData(PlayerEcsMediator playerEcsMediator, 
        BusinessEcsMediator businessEcsMediator)
    {
        UpdatePlayerData(playerEcsMediator);
        UpdateBusinessesData(businessEcsMediator);
    }

    private void UpdatePlayerData(PlayerEcsMediator playerEcsMediator)
    {
        _gameData.PlayerData.Balance = playerEcsMediator.GetPlayerBalance();
    }

    private void UpdateBusinessesData(BusinessEcsMediator businessEcsMediator)
    {
        var businessesDataCollection = businessEcsMediator.GetBusinessesGameData();
        foreach (var businessData in businessesDataCollection)
        {
            _gameData.BusinessesData[businessData.Id] = businessData;
        }
    }

    public void Save()
    {
        _saveDataHandler.SaveData(_gameData);
    }
}
}