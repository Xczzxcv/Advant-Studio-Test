using Core.Ecs.Components;
using Infrastructure;
using Leopotam.EcsLite;

namespace Core.Ecs.Systems
{
internal class ProgressCompletionToIncomeEventSystem 
    : EcsBaseRunSystem<ProgressCompletedEvent, IncomeSourceComponent>
{
    public ProgressCompletionToIncomeEventSystem(EnvironmentServices services) : base(services)
    { }

    protected override void ProcessComponents(ref ProgressCompletedEvent progressCompletedEvt,
        ref IncomeSourceComponent incomeSrc, EcsWorld world, int entity)
    {
        var incomePortionEntity = world.NewEntity();
        ref var incomePortion = ref world.AddComponent<IncomePortionComponent>(incomePortionEntity);
        incomePortion.IncomeAmount = incomeSrc.TotalIncomeAmount;
    }
}
}