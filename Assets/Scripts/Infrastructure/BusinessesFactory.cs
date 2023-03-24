using System.Collections.Generic;
using System.Linq;
using Core.Ecs;
using Core.Ecs.Components;
using Core.GameConfigs;
using Core.GameData;
using Leopotam.EcsLite;

namespace Infrastructure
{
internal class BusinessesFactory : IBusinessesFactory
{
    private readonly EcsWorld _world;
    private readonly IBusinessEntitiesProvider _businessEntitiesProvider;
    private readonly IBusinessUpgradesApplier _businessUpgradesApplier;

    public BusinessesFactory(EcsWorld world, 
        IBusinessEntitiesProvider businessEntitiesProvider,
        IBusinessUpgradesApplier businessUpgradesApplier)
    {
        _world = world;
        _businessEntitiesProvider = businessEntitiesProvider;
        _businessUpgradesApplier = businessUpgradesApplier;
    }

    public void CreateBusinessEntity(BusinessConfig businessConfig, BusinessData businessData)
    {
        var businessEntity = _world.NewEntity();
        
        ref var businessComponent = ref _world.AddComponent<BusinessComponent>(businessEntity);
        businessComponent.Id = businessConfig.Id;
        businessComponent.AppliedUpgrades = new List<string>();
        
        ref var businessProgressComp = ref _world.AddComponent<ProgressComponent>(businessEntity);
        businessProgressComp.ProgressAmount = businessData.Progress;
        businessProgressComp.ProgressPeriod = businessConfig.ProgressPeriod;
        businessProgressComp.IsActive = businessData.Lvl > 0;
        
        ref var businessIncomeComp = ref _world.AddComponent<IncomeSourceComponent>(businessEntity);
        businessIncomeComp.BaseIncomeAmount = businessConfig.BaseIncomeAmount;
        businessIncomeComp.IncomeSrcLvl = businessData.Lvl;
        businessIncomeComp.IncomeMultiplier = 1;

        ApplyBusinessUpgrades(businessConfig, businessData, ref businessComponent, businessEntity);
        _businessEntitiesProvider.Add(businessConfig.Id, businessEntity);
    }

    private void ApplyBusinessUpgrades(BusinessConfig businessConfig, BusinessData businessData,
        ref BusinessComponent businessComp, int businessEntity)
    {
        foreach (var businessUpgrade in businessConfig.Upgrades)
        {
            var isUpgradeActive = businessData.UpgradeIds.Contains(businessUpgrade.Id);
            if (!isUpgradeActive)
            {
                continue;
            }

            _businessUpgradesApplier.ApplyUpgradeToBusiness(businessEntity, businessUpgrade,
                ref businessComp, true);
        }
    }
}
}