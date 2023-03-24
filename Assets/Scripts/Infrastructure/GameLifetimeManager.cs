using Core.Ecs;
using Core.GameData;
using UnityEngine;

namespace Infrastructure
{
internal class GameLifetimeManager : MonoBehaviour
{
    private GameDataManager _gameDataManager;
    private PlayerEcsMediator _playerEcsMediator;
    private BusinessEcsMediator _businessEcsMediator;

    public void Init(GameDataManager gameDataManager, PlayerEcsMediator playerEcsMediator,
        BusinessEcsMediator businessEcsMediator)
    {
        _gameDataManager = gameDataManager;
        _playerEcsMediator = playerEcsMediator;
        _businessEcsMediator = businessEcsMediator;
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGameData();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGameData();
        }
    }

    private void OnDestroy()
    {
        SaveGameData();
    }

    private void SaveGameData()
    {
        _gameDataManager.UpdateData(_playerEcsMediator, _businessEcsMediator);
        _gameDataManager.Save();
    }
}
}