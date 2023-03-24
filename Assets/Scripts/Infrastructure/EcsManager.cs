using Core.Ecs.Components;
using Core.Ecs.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Infrastructure
{
internal class EcsManager : MonoBehaviour, IUpdatable
{
    private EcsWorld _world;
    private EcsSystems _systems;

    public void Init(EcsWorld world)
    {
        _world = world;
        var services = new EnvironmentServices();
        _systems = new EcsSystems(_world);
        _systems
            .Add(new ProgressUpdateSystem(services))
            .Add(new ProgressCompletionToIncomeEventSystem(services))
            .Add(new ProcessIncomePortionSystem(services))
            .DelHere<ProgressCompletedEvent>()
            .Init();
    }

    public void OnUpdate()
    {
        _systems?.Run();
    }
}
}