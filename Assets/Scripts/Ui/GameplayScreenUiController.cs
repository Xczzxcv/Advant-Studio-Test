using Core.Ecs;
using Core.GameConfigs;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui
{
internal class GameplayScreenUiController : UIBehaviour, IUpdatable
{
    [SerializeField] private PlayerDataUiController playerDataUiController;
    [SerializeField] private AllBusinessesUiController allBusinessesUiController;

    public void Init(EcsWorld world,
        PlayerEcsMediator playerEcsMediator,
        BusinessEcsMediator businessEcsMediator,
        IBusinessEntitiesProvider businessEntitiesProvider,
        IGameConfigCollectionsProvider gameConfigCollectionsProvider)
    {
        playerDataUiController.Init(playerEcsMediator);
        allBusinessesUiController.Init(world,
            businessEcsMediator,
            businessEntitiesProvider,
            gameConfigCollectionsProvider);
    }

    public void OnUpdate()
    {
        playerDataUiController.OnUpdate();
        allBusinessesUiController.OnUpdate();
    }
}
}