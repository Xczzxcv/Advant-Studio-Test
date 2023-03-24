using Core.Ecs;
using Core.Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.GameConfigs
{
[CreateAssetMenu(menuName = "Business Upgrade/Multiplier Upgrade Config", fileName = "BusinessMultiplierUpgrade", order = 0)]
internal class BusinessMultiplierUpgrade : BusinessUpgrade
{
    [SerializeField] private double additionalMultiplier;

    public double AdditionalMultiplier => additionalMultiplier;

    public override void Upgrade(EcsWorld world, int businessEntity)
    {
        ref var businessIncomeSrcComp = ref world.GetComponent<IncomeSourceComponent>(businessEntity);
        businessIncomeSrcComp.IncomeMultiplier += additionalMultiplier;
    }
}
}