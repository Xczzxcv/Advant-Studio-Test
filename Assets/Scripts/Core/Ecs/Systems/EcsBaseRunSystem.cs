using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine.Profiling;

namespace Core.Ecs.Systems
{
internal abstract class EcsBaseRunSystem<TComponent> : EcsBaseRunSystemInternal 
    where TComponent : struct
{
    protected EcsPool<TComponent> Pool;

    protected EcsBaseRunSystem(EnvironmentServices services)
        : base(services)
    { }

    public override void Init(IEcsSystems systems)
    {
        Filter = systems.GetWorld().Filter<TComponent>().End();
        Pool = systems.GetWorld().GetPool<TComponent>();
    }

    public override void Run(IEcsSystems systems)
    {
        Profiler.BeginSample(GetType().Name);
        ProcessComponents(Filter, Pool, systems.GetWorld());
        Profiler.EndSample();
    }

    private void ProcessComponents(EcsFilter filter, EcsPool<TComponent> components, EcsWorld world)
    {
        foreach (var entity in filter)
        {
            ref var component = ref components.Get(entity);
            ProcessComponent(ref component, world, entity);
        }
    }

    protected abstract void ProcessComponent(ref TComponent component, EcsWorld world, int entity);
}
}
