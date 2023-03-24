using UnityEngine;

namespace Core.GameConfigs
{
[CreateAssetMenu(menuName = "Business Upgrade/Multiplier Upgrade View Config", fileName = "BusinessMultiplierUpgradeViewConfig", order = 0)]
internal class BusinessMultiplierUpgradeViewConfig : BusinessUpgradeViewConfig<BusinessMultiplierUpgrade>
{
    protected override string GetEffectDescriptionInternal(BusinessMultiplierUpgrade concreteUpgradeConfig)
    {
        var additionalMultiplier = concreteUpgradeConfig.AdditionalMultiplier;
        return string.Format(effectDescription, additionalMultiplier);
    }
}
}