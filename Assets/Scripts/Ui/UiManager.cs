using Core.Ecs;
using Core.GameConfigs;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ui
{
internal class UiManager : MonoBehaviour, IUpdatable
{
    [SerializeField] private GameplayScreenUiController gameplayScreenUiControllerPrefab;
    [SerializeField] private Transform uiRoot;
    
    private GameplayScreenUiController _gameplayScreenUiController;

    public void Init(EcsWorld world,
        PlayerEcsMediator playerEcsMediator,
        BusinessEcsMediator businessEcsMediator,
        IBusinessEntitiesProvider businessEntitiesProvider,
        IGameConfigCollectionsProvider gameConfigCollectionsProvider)
    {
        _gameplayScreenUiController = Instantiate(gameplayScreenUiControllerPrefab, uiRoot);
        _gameplayScreenUiController.Init(world,
            playerEcsMediator,
            businessEcsMediator,
            businessEntitiesProvider,
            gameConfigCollectionsProvider);
    }

    public void OnUpdate()
    {
        if (_gameplayScreenUiController)
        {
            _gameplayScreenUiController.OnUpdate();
        }
    }
}
}