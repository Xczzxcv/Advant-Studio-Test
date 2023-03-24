using Core.Ecs.Components;
using Core.GameConfigs;

namespace Infrastructure
{
internal interface IBusinessUpgradesApplier
{
    void ApplyUpgradeToBusiness(string businessId, string businessUpgradeId, bool isFree = false);
    void ApplyUpgradeToBusiness(int businessEntity, IBusinessUpgrade businessUpgrade,
        ref BusinessComponent businessComp, bool isFree = false);
    bool CanApplyUpgradeToBusiness(string businessId, string businessUpgradeId);
}
}