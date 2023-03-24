using Core.Ecs.Components;
using Infrastructure;
using Leopotam.EcsLite;

namespace Core.Ecs.Systems
{
internal class ProgressUpdateSystem : EcsBaseRunSystem<ProgressComponent>
{
    public ProgressUpdateSystem(EnvironmentServices services) : base(services)
    { }

    protected override void ProcessComponent(ref ProgressComponent progress, EcsWorld world, int entity)
    {
        if (!progress.IsActive)
        {
            return;
        }

        progress.ProgressAmount += Services.DeltaTime / progress.ProgressPeriod;
        if (progress.IsCompleted)
        {
            progress.ProgressAmount %= (int) progress.ProgressAmount;
            world.AddComponent<ProgressCompletedEvent>(entity);
        }
    }
}
}