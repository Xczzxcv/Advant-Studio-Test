using UnityEngine;

namespace Core.GameConfigs
{
internal abstract class BusinessUpgradeViewConfig<TUpgradeConfig> : BusinessUpgradeViewConfigBase
    where TUpgradeConfig : class, IBusinessUpgrade
{
    [SerializeField]
    private string _name;

    [SerializeField]
    protected string effectDescription;

    public override string Name => _name;

    public override string GetEffectDescription(IBusinessUpgrade upgradeConfig)
    {
        if (upgradeConfig.Id != Id)
        {
            return ReturnError(upgradeConfig);
        }

        if (upgradeConfig is not TUpgradeConfig concreteUpgradeConfig)
        {
            return ReturnError(upgradeConfig);
        }

        return GetEffectDescriptionInternal(concreteUpgradeConfig);
    }

    private static string ReturnError(IBusinessUpgrade upgradeConfig)
    {
        Debug.LogError($"Wrong business upgrade '{upgradeConfig.Id}' " +
                       $"type {upgradeConfig.GetType()} (expected {typeof(TUpgradeConfig)})");
        return string.Empty;
    }

    protected abstract string GetEffectDescriptionInternal(TUpgradeConfig concreteUpgradeConfig);
}
}