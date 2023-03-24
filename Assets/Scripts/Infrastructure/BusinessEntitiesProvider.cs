using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Infrastructure
{
internal class BusinessEntitiesProvider : IBusinessEntitiesProvider
{
    private readonly EcsWorld _world;
    private readonly Dictionary<string, EcsPackedEntity> _businessEntities = new();
    private readonly List<(string, int)> _businessEntitiesCache = new();

    public BusinessEntitiesProvider(EcsWorld world)
    {
        _world = world;
    }

    public bool TryGetEntity(string businessId, out int businessEntity)
    {
        businessEntity = default;
        if (!_businessEntities.TryGetValue(businessId, out var businessPackedEntity))
        {
            return false;
        }

        return businessPackedEntity.Unpack(_world, out businessEntity);
    }

    public void Add(string businessId, int businessEntity)
    {
        _businessEntities.Add(businessId, _world.PackEntity(businessEntity));
    }

    public IReadOnlyCollection<(string businessId, int businessEntity)> GetAllBusinessEntities()
    {
        _businessEntitiesCache.Clear();
        foreach (var (businessId, businessPackedEntity) in _businessEntities)
        {
            if (!businessPackedEntity.Unpack(_world, out var businessEntity))
            {
                continue;
            }

            _businessEntitiesCache.Add((businessId, businessEntity));
        }

        return _businessEntitiesCache;
    }
}
}