namespace Core.GameConfigs
{
internal interface IBusinessUpgradeViewConfig : IGameConfig
{
    string Name { get; }
    string GetEffectDescription(IBusinessUpgrade upgradeConfig);
}
}