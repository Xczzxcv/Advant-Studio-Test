using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Core.Ecs
{
public static class LeoEcsExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TComponent GetComponent<TComponent>(this EcsWorld world, int entity)
        where TComponent : struct
    {
        var components = world.GetPool<TComponent>();
        return ref components.Get(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasComponent<TComponent>(this EcsWorld world, int entity)
        where TComponent : struct
    {
        var components = world.GetPool<TComponent>();
        return components.Has(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TComponent AddComponent<TComponent>(this EcsWorld world, int entity)
        where TComponent : struct
    {
        var components = world.GetPool<TComponent>();
        return ref components.Add(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DelComponent<TComponent>(this EcsWorld world, int entity)
        where TComponent : struct
    {
        var components = world.GetPool<TComponent>();
        components.Del(entity);
    }

    public static ref TComponent GetOrAddComponent<TComponent>(this EcsWorld world, int entity)
        where TComponent : struct
    {
        var pool = world.GetPool<TComponent>();
        return ref pool.GetOrAdd(entity);
    }

    public static ref TComponent GetOrAdd<TComponent>(this EcsPool<TComponent> pool, int entity) 
        where TComponent : struct
    {
        return ref pool.Has(entity)
            ? ref pool.Get(entity)
            : ref pool.Add(entity);
    }
}
}