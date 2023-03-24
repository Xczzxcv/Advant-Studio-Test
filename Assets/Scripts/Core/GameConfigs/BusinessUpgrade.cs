using Leopotam.EcsLite;
using UnityEngine;

namespace Core.GameConfigs
{
internal abstract class BusinessUpgrade : ScriptableGameConfig, IBusinessUpgrade
{
    [SerializeField] private double price;

    public double Price => price;

    public abstract void Upgrade(EcsWorld world, int businessEntity);
}
}