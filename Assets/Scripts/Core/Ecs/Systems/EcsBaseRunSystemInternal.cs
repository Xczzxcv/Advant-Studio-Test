using Infrastructure;
using Leopotam.EcsLite;

namespace Core.Ecs.Systems
{
internal abstract class EcsBaseRunSystemInternal : IEcsRunSystem, IEcsInitSystem
{
    protected readonly EnvironmentServices Services;
    protected EcsFilter Filter;

    protected EcsBaseRunSystemInternal(EnvironmentServices services)
    {
        Services = services;
    }

    public virtual void Init(IEcsSystems systems)
    { }

    public abstract void Run(IEcsSystems systems);
}
}