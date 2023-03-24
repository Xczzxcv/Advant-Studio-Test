using System;
using Core.Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Ecs
{
internal class PlayerEcsMediator : EcsMediator
{
    private readonly EcsPackedEntity _playerBalanceEntity;

    public PlayerEcsMediator(EcsPackedEntity playerBalanceEntity, EcsWorld world) : base(world)
    {
        _playerBalanceEntity = playerBalanceEntity;
    }

    private ref PlayerBalanceComponent GetPlayerBalanceComponent()
    {
        var playerBalancePool = World.GetPool<PlayerBalanceComponent>();
        if (!_playerBalanceEntity.Unpack(World, out var playerBalanceEntity))
        {
            throw new NotImplementedException("Can't find player balance");
        }

        return ref playerBalancePool.Get(playerBalanceEntity);
    }

    public double GetPlayerBalance()
    {
        return GetPlayerBalanceComponent().Balance;
    }

    public void ChargePlayer(double chargeAmount)
    {
        ref var playerBalanceComponent = ref GetPlayerBalanceComponent();

        Debug.Assert(playerBalanceComponent.Balance >= chargeAmount);

        var oldBalance = playerBalanceComponent.Balance; 
        playerBalanceComponent.Balance -= chargeAmount;
        
        Debug.Log($"Player was charged {chargeAmount} " +
                  $"(balance: {oldBalance} -> {playerBalanceComponent.Balance})");
    }
}
}