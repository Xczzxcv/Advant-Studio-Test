using Core.GameConfigs;
using Core.GameData;

namespace Infrastructure
{
internal interface IBusinessesFactory
{
    void CreateBusinessEntity(BusinessConfig businessConfig, BusinessData businessData);
}
}