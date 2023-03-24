using UnityEngine;

namespace Core.GameConfigs
{
internal partial class GameConfigsProvider : MonoBehaviour, IGameConfigCollectionsProvider
{
    [SerializeField] private BusinessConfig[] businesses;
    [SerializeField] private BusinessViewConfig[] businessViews;
    [SerializeField] private BusinessUpgradeViewConfigBase[] businessUpgradeViews;
    
    public IReadOnlyConfigCollection<BusinessConfig> BusinessConfigs => _businessConfigs;
    public IReadOnlyConfigCollection<IBusinessUpgrade> BusinessUpgradeConfigs => _businessUpgrades;
    public IReadOnlyConfigCollection<BusinessViewConfig> BusinessViewConfigs => _businessViewConfigs;
    public IReadOnlyConfigCollection<IBusinessUpgradeViewConfig> BusinessUpgradeViewConfigs =>
        _businessUpgradeViewConfigs;

    private readonly GameConfigCollection<BusinessConfig> _businessConfigs = new();
    private readonly GameConfigCollection<IBusinessUpgrade> _businessUpgrades = new();
    private readonly GameConfigCollection<BusinessViewConfig> _businessViewConfigs = new();
    private readonly GameConfigCollection<IBusinessUpgradeViewConfig> _businessUpgradeViewConfigs = new();

    public void Init()
    {
        InitBusinesses();
        InitBusinessUpgrades();
        InitBusinessViews();
        InitBusinessUpgradeViews();
    }

    private void InitBusinesses()
    {
        foreach (var businessConfig in businesses)
        {
            _businessConfigs.Add(businessConfig.Id, businessConfig);
        }
    }

    private void InitBusinessUpgrades()
    {
        foreach (var businessConfig in businesses)
        {
            foreach (var businessUpgrade in businessConfig.Upgrades)
            {
                _businessUpgrades.Add(businessUpgrade.Id, businessUpgrade);
            }
        }
    }

    private void InitBusinessViews()
    {
        foreach (var businessViewConfig in businessViews)
        {
            _businessViewConfigs.Add(businessViewConfig.Id, businessViewConfig);
        }
    }

    private void InitBusinessUpgradeViews()
    {
        foreach (var businessUpgradeViewConfig in businessUpgradeViews)
        {
            _businessUpgradeViewConfigs.Add(businessUpgradeViewConfig.Id, businessUpgradeViewConfig);
        }
    }
}
}