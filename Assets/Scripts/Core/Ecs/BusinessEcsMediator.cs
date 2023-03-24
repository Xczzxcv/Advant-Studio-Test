using System.Collections.Generic;
using Core.Ecs.Components;
using Core.GameConfigs;
using Core.GameData;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Ecs
{
internal class BusinessEcsMediator : EcsMediator, IBusinessUpgradesApplier, IBusinessLevelIncreaser
{
    private readonly IBusinessEntitiesProvider _businessEntitiesProvider;
    private readonly IGameConfigCollectionsProvider _gameConfigCollectionsProvider;
    private readonly PlayerEcsMediator _playerEcsMediator;
    private readonly List<BusinessData> _businessDataCache = new();

    public BusinessEcsMediator(EcsWorld world, 
        IBusinessEntitiesProvider businessEntitiesProvider,
        IGameConfigCollectionsProvider gameConfigCollectionsProvider,
        PlayerEcsMediator playerEcsMediator) 
        : base(world)
    {
        _businessEntitiesProvider = businessEntitiesProvider;
        _gameConfigCollectionsProvider = gameConfigCollectionsProvider;
        _playerEcsMediator = playerEcsMediator;
    }

    public void IncreaseBusinessLevel(string businessId, bool isFree = false)
    {
        if (!TryGetBusinessEntity(businessId, out var businessEntity))
        {
            return;
        }

        ref var incomeSourceComp = ref World.GetComponent<IncomeSourceComponent>(businessEntity);
        var oldLvl = incomeSourceComp.IncomeSrcLvl;

        incomeSourceComp.IncomeSrcLvl++;

        if (!isFree)
        {
            var nextLevelBusinessCost = GetNextLevelBusinessCost(businessId, oldLvl);
            _playerEcsMediator.ChargePlayer(nextLevelBusinessCost);
        }
        
        Debug.Log($"Business '{businessId}' lvl was increased ({incomeSourceComp.IncomeSrcLvl})");
    }

    public void PurchaseBusiness(string businessId)
    {
        IncreaseBusinessLevel(businessId);

        if (!TryGetBusinessEntity(businessId, out var businessEntity))
        {
            return;
        }

        ref var progressComponent = ref World.GetComponent<ProgressComponent>(businessEntity);
        progressComponent.IsActive = true;
        
        Debug.Log($"Business '{businessId}' was purchased");
    }

    public void ApplyUpgradeToBusiness(string businessId, string businessUpgradeId, bool isFree = false)
    {
        if (!TryGetBusinessEntity(businessId, out var businessEntity))
        {
            return;
        }

        if (!_gameConfigCollectionsProvider.BusinessUpgradeConfigs.TryGetValue(businessUpgradeId, 
                out var businessUpgrade))
        {
            Debug.LogError($"Can't find business upgrade '{businessUpgradeId}'");
            return;
        }

        ref var businessComponent = ref World.GetComponent<BusinessComponent>(businessEntity);
        ApplyUpgradeToBusiness(businessEntity, businessUpgrade, ref businessComponent, isFree);
    }

    public void ApplyUpgradeToBusiness(int businessEntity, IBusinessUpgrade businessUpgrade,
        ref BusinessComponent businessComp, bool isFree = false)
    {
        ref var businessComponent = ref World.GetComponent<BusinessComponent>(businessEntity);
        businessUpgrade.Upgrade(World, businessEntity);
        businessComponent.AppliedUpgrades.Add(businessUpgrade.Id);

        if (!isFree)
        {
            _playerEcsMediator.ChargePlayer(businessUpgrade.Price);
        }

        Debug.Log($"Business '{businessComp.Id}' upgrade '{businessUpgrade.Id}' was applied");
    }

    private bool TryGetBusinessEntity(string businessId, out int businessEntity)
    {
        if (!_businessEntitiesProvider.TryGetEntity(businessId, out businessEntity))
        {
            Debug.LogError($"Can't find business '{businessId}' entity");
            return false;
        }

        return true;
    }

    public bool CanIncreaseBusinessLevel(string businessId)
    {
        if (!TryGetBusinessEntity(businessId, out var businessEntity))
        {
            return false;
        }

        var playerBalance = _playerEcsMediator.GetPlayerBalance();
        
        var incomeSourceComp = World.GetComponent<IncomeSourceComponent>(businessEntity);
        var currentBusinessLvl = incomeSourceComp.IncomeSrcLvl;
        var lvlUpBusinessCost = GetNextLevelBusinessCost(businessId, currentBusinessLvl);
        
        return playerBalance >= lvlUpBusinessCost;
    }

    public bool CanPurchaseBusiness(string businessId)
    {
        return CanIncreaseBusinessLevel(businessId);
    }

    public bool CanApplyUpgradeToBusiness(string businessId, string businessUpgradeId)
    {
        if (!TryGetBusinessEntity(businessId, out var businessEntity))
        {
            return false;
        }

        var businessComponent = World.GetComponent<BusinessComponent>(businessEntity);
        if (businessComponent.AppliedUpgrades.Contains(businessUpgradeId))
        {
            return false;
        }

        if (!_gameConfigCollectionsProvider.BusinessUpgradeConfigs.TryGetValue(businessUpgradeId, out var businessUpgrade))
        {
            Debug.LogError($"Can't find business upgrade '{businessUpgradeId}'");
            return false;
        }

        var playerBalance = _playerEcsMediator.GetPlayerBalance();
        return playerBalance >= businessUpgrade.Price;
    }

    public IReadOnlyCollection<BusinessData> GetBusinessesGameData()
    {
        _businessDataCache.Clear();
        foreach (var (_, businessEntity) in _businessEntitiesProvider.GetAllBusinessEntities())
        {
            var businessData = GetBusinessGameData(businessEntity);
            _businessDataCache.Add(businessData);
        }

        return _businessDataCache;
    }

    private BusinessData GetBusinessGameData(int businessEntity)
    {
        var businessComp = World.GetComponent<BusinessComponent>(businessEntity);
        var progressComp = World.GetComponent<ProgressComponent>(businessEntity);
        var incomeSrcComp = World.GetComponent<IncomeSourceComponent>(businessEntity);
        return new BusinessData
        {
            Id = businessComp.Id,
            UpgradeIds = businessComp.AppliedUpgrades.ToArray(),
            Progress = progressComp.ProgressAmount,
            Lvl = incomeSrcComp.IncomeSrcLvl,
        };
    }

    public double GetNextLevelBusinessCost(string businessId, int currentBusinessLvl)
    {
        var businessConfig = _gameConfigCollectionsProvider.BusinessConfigs[businessId];
        var nextLvlCost = BusinessFormulas.NextBusinessLvlCostFormula(currentBusinessLvl, 
            businessConfig.BasePriceAmount);
        return nextLvlCost;
    }

    public double GetBusinessIncomeAtLvl(string businessId, int businessLvl)
    {
        if (!TryGetBusinessEntity(businessId, out var businessEntity))
        {
            Debug.LogError($"Can't get business '{businessId}' income at lvl {businessLvl}");
            return 0;
        }

        var incomeSrcComp = World.GetComponent<IncomeSourceComponent>(businessEntity);

        return BusinessFormulas.BusinessTotalIncome(incomeSrcComp.BaseIncomeAmount,
            incomeSrcComp.IncomeMultiplier, businessLvl);
    }
}
}