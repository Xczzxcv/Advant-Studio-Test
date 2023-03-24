using Core.Ecs;
using Core.Ecs.Components;
using Core.GameConfigs;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ui
{
internal class BusinessUiController : MonoBehaviour
{
    [SerializeField] private BusinessView businessView;

    private EcsWorld _world;
    private string _businessId;
    private BusinessEcsMediator _businessEcsMediator;
    private IBusinessEntitiesProvider _businessEntitiesProvider;
    private IGameConfigCollectionsProvider _gameConfigCollectionsProvider;

    private void Awake()
    {
        businessView.BusinessLvlUpBtnClick += OnBusinessLvlUpBtnClick;
        businessView.BusinessUpgradeBtnClick += OnBusinessUpgradeBtnClick;
    }

    public void Init(EcsWorld world,
        BusinessEcsMediator businessEcsMediator,
        IBusinessEntitiesProvider businessEntitiesProvider,
        IGameConfigCollectionsProvider gameConfigCollectionsProvider)
    {
        _world = world;
        _businessEcsMediator = businessEcsMediator;
        _businessEntitiesProvider = businessEntitiesProvider;
        _gameConfigCollectionsProvider = gameConfigCollectionsProvider;
    }

    public void Setup(string businessId)
    {
        _businessId = businessId;
        UpdateView();
    }

    private void UpdateView()
    {
        if (!TryBuildViewConfig(out var viewConfig))
        {
            Debug.LogError($"Can't build business '{_businessId}' view config");
            return;
        }

        businessView.Setup(viewConfig);
    }

    private bool TryBuildViewConfig(out BusinessView.Config viewConfig)
    {
        if (!_businessEntitiesProvider.TryGetEntity(_businessId, out var businessEntity))
        {
            viewConfig = default;
            return false;
        }

        var businessComp = _world.GetComponent<BusinessComponent>(businessEntity);
        var incomeSrcComp = _world.GetComponent<IncomeSourceComponent>(businessEntity);
        var progressComp = _world.GetComponent<ProgressComponent>(businessEntity);
        var businessConfig = _gameConfigCollectionsProvider.BusinessConfigs[businessComp.Id];
        var businessViewConfig = _gameConfigCollectionsProvider.BusinessViewConfigs[businessComp.Id];

        var businessLevel = incomeSrcComp.IncomeSrcLvl;
        var businessViewIncome = businessLevel > 0
            ? incomeSrcComp.TotalIncomeAmount
            : _businessEcsMediator.GetBusinessIncomeAtLvl(_businessId, 1);

        viewConfig = new BusinessView.Config
        {
            Id = _businessId,
            Name = businessViewConfig.Name,
            Progress = (float) progressComp.ProgressAmount,
            Level = businessLevel,
            Income = businessViewIncome,
            CanLvlUp = _businessEcsMediator.CanIncreaseBusinessLevel(_businessId),
            NextLevelCost = _businessEcsMediator.GetNextLevelBusinessCost(_businessId, businessLevel),
            Upgrades = GetUpgradeViewConfigs(businessConfig, businessComp),
        };
        return true;
    }

    private BusinessUpgradeView.Config[] GetUpgradeViewConfigs(BusinessConfig businessConfig,
        BusinessComponent businessComp)
    {
        var businessUpgradeConfigs = new BusinessUpgradeView.Config[businessConfig.Upgrades.Length];
        for (var i = 0; i < businessConfig.Upgrades.Length; i++)
        {
            var businessUpgrade = businessConfig.Upgrades[i];
            var businessUpgradeId = businessUpgrade.Id;
            var businessUpgradeViewConfigsCollection = _gameConfigCollectionsProvider.BusinessUpgradeViewConfigs;
            var businessUpgradeViewConfig = businessUpgradeViewConfigsCollection[businessUpgradeId];
            businessUpgradeConfigs[i] = new BusinessUpgradeView.Config
            {
                Id = businessUpgradeId,
                Price = businessUpgrade.Price,
                IsApplied = businessComp.AppliedUpgrades.Contains(businessUpgradeId),
                CanBeApplied = _businessEcsMediator.CanApplyUpgradeToBusiness(_businessId,
                    businessUpgradeId),
                Name = businessUpgradeViewConfig.Name,
                EffectDescription = businessUpgradeViewConfig.GetEffectDescription(businessUpgrade),
            };
        }

        return businessUpgradeConfigs;
    }

    public void UpdateBusiness()
    {
        UpdateView();
    }

    private void OnBusinessLvlUpBtnClick(int currentLvl)
    {
        var isBusinessPurchased = currentLvl > 0;
        if (isBusinessPurchased)
        {
            if (!_businessEcsMediator.CanIncreaseBusinessLevel(_businessId))
            {
                return;
            }
        
            _businessEcsMediator.IncreaseBusinessLevel(_businessId);
            UpdateView();
        }
        else
        {
            if (!_businessEcsMediator.CanPurchaseBusiness(_businessId))
            {
                return;
            }
            
            _businessEcsMediator.PurchaseBusiness(_businessId);
            UpdateView();
        }
    }

    private void OnBusinessUpgradeBtnClick(string businessUpgradeId)
    {
        if (!_businessEcsMediator.CanApplyUpgradeToBusiness(_businessId, businessUpgradeId))
        {
            return;
        }

        _businessEcsMediator.ApplyUpgradeToBusiness(_businessId, businessUpgradeId);
        UpdateView();
    }

    private void OnDestroy()
    {
        businessView.BusinessUpgradeBtnClick -= OnBusinessUpgradeBtnClick;
    }
}
}