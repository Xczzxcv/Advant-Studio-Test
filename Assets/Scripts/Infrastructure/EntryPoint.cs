using Core.Ecs;
using Core.GameConfigs;
using Core.GameData;
using Leopotam.EcsLite;
using Ui;
using UnityEngine;

namespace Infrastructure
{
internal class EntryPoint : MonoBehaviour
{
    [SerializeField] private EcsManager ecsManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private GameConfigsProvider gameConfigsProvider;
    [SerializeField] private GameLifetimeManager gameLifetimeManager;
    [SerializeField] private GameUpdater gameUpdater;

    private void Start()
    {
        var ecsWorld = new EcsWorld();
        gameConfigsProvider.Init();

        InitGameData(gameConfigsProvider, out var gameDataManager);
        InitPlayer(ecsWorld, gameDataManager, out var playerEcsMediator);
        InitBusinesses(gameDataManager, playerEcsMediator, ecsWorld, gameConfigsProvider,
            out var businessEntitiesProvider, out var businessEcsMediator);

        ecsManager.Init(ecsWorld);
        uiManager.Init(ecsWorld, playerEcsMediator, businessEcsMediator, businessEntitiesProvider,
            gameConfigsProvider);
        gameLifetimeManager.Init(gameDataManager, playerEcsMediator, businessEcsMediator);


        gameUpdater.Add(ecsManager, uiManager);
        Debug.Log("Initialization ended");
    }

    private static void InitPlayer(EcsWorld ecsWorld, IPlayerDataProvider playerDataProvider,
        out PlayerEcsMediator playerEcsMediator)
    {
        var playerInitializer = new PlayerInitializer(ecsWorld, playerDataProvider);
        playerEcsMediator = playerInitializer.Init();
    }

    private static void InitBusinesses(GameDataManager gameDataManager, PlayerEcsMediator playerEcsMediator, 
        EcsWorld ecsWorld, GameConfigsProvider gameConfigsProvider, 
        out IBusinessEntitiesProvider businessEntitiesProvider, 
        out BusinessEcsMediator businessEcsMediator)
    {
        var businessesInitializer = new BusinessesInitializer(gameConfigsProvider,
            gameDataManager, playerEcsMediator);
        (businessEntitiesProvider , businessEcsMediator) = businessesInitializer.InitBusinesses(ecsWorld);
    }

    private static void InitGameData(GameConfigsProvider gameConfigsProvider, 
        out GameDataManager gameDataManager)
    {
        var saveDataHandler = new LocalJsonSaveDataHandler();
        var gameDataInitializer = new GameDataInitializer(gameConfigsProvider);
        gameDataManager = new GameDataManager(saveDataHandler, gameDataInitializer);
    }
}
}