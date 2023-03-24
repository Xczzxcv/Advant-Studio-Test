using Core.GameConfigs;
using Core.GameData;

namespace Infrastructure
{
internal class BusinessesSpawner : IBusinessesSpawner
{
    private readonly IGameConfigCollectionsProvider _gameConfigCollectionsProvider;
    private readonly IBusinessesFactory _businessesFactory;
    private readonly IGameDataCollectionProvider<BusinessData> _businessesDataProvider;

    public BusinessesSpawner(
        IGameConfigCollectionsProvider gameConfigCollectionsProvider,
        IBusinessesFactory businessesFactory, 
        IGameDataCollectionProvider<BusinessData> businessesDataProvider)
    {
        _gameConfigCollectionsProvider = gameConfigCollectionsProvider;
        _businessesFactory = businessesFactory;
        _businessesDataProvider = businessesDataProvider;
    }
    
    public void SpawnBusinesses()
    {
        var businessesData = _businessesDataProvider.GetData();
        foreach (var businessConfig in _gameConfigCollectionsProvider.BusinessConfigs.Values)
        {
            if (!businessesData.TryGetValue(businessConfig.Id, out var businessData))
            {
                businessData = BusinessData.BuildDefault(businessConfig.Id);
            }

            _businessesFactory.CreateBusinessEntity(businessConfig, businessData);
        }
    }
}
}