namespace Core.GameConfigs
{
internal interface IGameConfigCollectionsProvider
{
    IReadOnlyConfigCollection<BusinessConfig> BusinessConfigs { get; }
    IReadOnlyConfigCollection<IBusinessUpgrade> BusinessUpgradeConfigs { get; }
    IReadOnlyConfigCollection<BusinessViewConfig> BusinessViewConfigs { get; }
    IReadOnlyConfigCollection<IBusinessUpgradeViewConfig> BusinessUpgradeViewConfigs { get; }
}
}