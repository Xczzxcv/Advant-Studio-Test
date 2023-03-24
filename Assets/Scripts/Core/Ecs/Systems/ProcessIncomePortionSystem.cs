using Core.Ecs.Components;
using Infrastructure;
using Leopotam.EcsLite;

namespace Core.Ecs.Systems
{
internal class ProcessIncomePortionSystem : EcsBaseRunSystem<IncomePortionComponent>
{
    private EcsFilter _playerBalanceFilter;
    private EcsPool<PlayerBalanceComponent> _playerBalancePool;

    public ProcessIncomePortionSystem(EnvironmentServices services) : base(services)
    { }

    public override void Init(IEcsSystems systems)
    {
        base.Init(systems);

        var world = systems.GetWorld();
        _playerBalanceFilter = world.Filter<PlayerBalanceComponent>().End();
        _playerBalancePool = world.GetPool<PlayerBalanceComponent>();
    }

    protected override void ProcessComponent(ref IncomePortionComponent incomePortion, EcsWorld world, int entity)
    {
        foreach (var playerBalanceEntity in _playerBalanceFilter)
        {
            IncreasePlayerBalance(playerBalanceEntity, incomePortion);
        }

        Pool.Del(entity);
    }

    private void IncreasePlayerBalance(int playerBalanceEntity, IncomePortionComponent incomePortion)
    {
        ref var playerBalance = ref _playerBalancePool.Get(playerBalanceEntity);
        playerBalance.Balance += incomePortion.IncomeAmount;
    }
}
}