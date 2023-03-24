using Leopotam.EcsLite;

namespace Core.Ecs
{
internal abstract class EcsMediator
{
    protected readonly EcsWorld World;

    protected EcsMediator(EcsWorld world)
    {
        World = world;
    }
}
}