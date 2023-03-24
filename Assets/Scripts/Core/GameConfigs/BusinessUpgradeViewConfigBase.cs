namespace Core.GameConfigs
{
internal abstract class BusinessUpgradeViewConfigBase : ScriptableGameConfig, IBusinessUpgradeViewConfig
{
    public abstract string Name { get; }
    public abstract string GetEffectDescription(IBusinessUpgrade upgradeConfig);
}
}