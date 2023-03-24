using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine.Profiling;

namespace Core.Ecs.Systems
{
internal abstract class EcsBaseRunSystem<TComponent1, TComponent2> : EcsBaseRunSystemInternal 
    where TComponent1 : struct
    where TComponent2 : struct
{
    protected EcsPool<TComponent1> Pool1;
    protected EcsPool<TComponent2> Pool2;

    protected EcsBaseRunSystem(EnvironmentServices services)
        : base(services)
    { }

    public override void Init(IEcsSystems systems)
    {
        Filter = systems.GetWorld().Filter<TComponent1>().Inc<TComponent2>().End();
        Pool1 = systems.GetWorld().GetPool<TComponent1>();
        Pool2 = systems.GetWorld().GetPool<TComponent2>();
    }

    public override void Run(IEcsSystems systems)
    {
        Profiler.BeginSample(GetType().Name);
        ProcessComponents(Filter, Pool1, Pool2, systems.GetWorld());
        Profiler.EndSample();
    }

    private void ProcessComponents(EcsFilter filter, EcsPool<TComponent1> pool1,
        EcsPool<TComponent2> pool2, EcsWorld world)
    {
        foreach (var entity in filter)
        {
            ref var component1 = ref pool1.Get(entity);
            ref var component2 = ref pool2.Get(entity);
            ProcessComponents(ref component1, ref component2, world, entity);
        }
    }

    protected abstract void ProcessComponents(ref TComponent1 component1, ref TComponent2 component2,
        EcsWorld world, int entity);
}
}