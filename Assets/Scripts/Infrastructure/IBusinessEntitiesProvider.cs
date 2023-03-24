using System.Collections.Generic;

namespace Infrastructure
{
internal interface IBusinessEntitiesProvider
{
    bool TryGetEntity(string businessId, out int businessEntity);
    void Add(string businessId, int businessEntity);
    IReadOnlyCollection<(string businessId, int businessEntity)> GetAllBusinessEntities();
}
}