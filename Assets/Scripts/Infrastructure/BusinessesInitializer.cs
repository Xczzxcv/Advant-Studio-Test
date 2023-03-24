using Core.Ecs;
using Core.GameConfigs;
using Core.GameData;
using Leopotam.EcsLite;

namespace Infrastructure
{
internal class BusinessesInitializer
{
    private readonly IGameConfigCollectionsProvider _gameConfigCollectionsProvider;
    private readonly IGameDataCollectionProvider<BusinessData> _businessesDataProvider;
    private readonly PlayerEcsMediator _playerEcsMediator;

    public BusinessesInitializer(
        IGameConfigCollectionsProvider gameConfigCollectionsProvider,
        IGameDataCollectionProvider<BusinessData> businessesDataProvider,
        PlayerEcsMediator playerEcsMediator)
    {
        _gameConfigCollectionsProvider = gameConfigCollectionsProvider;
        _businessesDataProvider = businessesDataProvider;
        _playerEcsMediator = playerEcsMediator;
    }

    public (IBusinessEntitiesProvider, BusinessEcsMediator) InitBusinesses(EcsWorld ecsWorld)
    {
        var businessEntitiesProvider = new BusinessEntitiesProvider(ecsWorld);
        var businessEcsMediator = new BusinessEcsMediator(ecsWorld,
            businessEntitiesProvider,
            _gameConfigCollectionsProvider,
            _playerEcsMediator);
        var businessesFactory = new BusinessesFactory(ecsWorld, businessEntitiesProvider, 
            businessEcsMediator);
        var businessesSpawner = new BusinessesSpawner(_gameConfigCollectionsProvider, businessesFactory, 
            _businessesDataProvider);
        businessesSpawner.SpawnBusinesses();

        return (businessEntitiesProvider, businessEcsMediator);
    }
}
}