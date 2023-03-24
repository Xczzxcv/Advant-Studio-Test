using Core.Ecs;
using Core.Ecs.Components;
using Core.GameData;
using Leopotam.EcsLite;

namespace Infrastructure
{
internal class PlayerInitializer
{
    private readonly EcsWorld _world;
    private readonly IPlayerDataProvider _playerDataProvider;

    public PlayerInitializer(EcsWorld world, IPlayerDataProvider playerDataProvider)
    {
        _world = world;
        _playerDataProvider = playerDataProvider;
    }

    public PlayerEcsMediator Init()
    {
        var playerBalanceEntity = InitPlayerBalance();
        return InitPlayerEcsMediator(playerBalanceEntity);
    }

    private int InitPlayerBalance()
    {
        var playerData = _playerDataProvider.GetData();
        var playerBalanceEntity = _world.NewEntity();
        ref var playerBalance = ref _world.AddComponent<PlayerBalanceComponent>(playerBalanceEntity);
        playerBalance.Balance = playerData.Balance;
        return playerBalanceEntity;
    }

    private PlayerEcsMediator InitPlayerEcsMediator(int playerBalanceEntity)
    {
        var playerEcsMediator = new PlayerEcsMediator(_world.PackEntity(playerBalanceEntity), _world);
        return playerEcsMediator;
    }
}
}