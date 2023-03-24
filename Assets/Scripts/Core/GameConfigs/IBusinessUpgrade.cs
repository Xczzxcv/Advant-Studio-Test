using Leopotam.EcsLite;

namespace Core.GameConfigs
{
internal interface IBusinessUpgrade : IGameConfig
{
    public double Price { get; }
    public void Upgrade(EcsWorld world, int businessEntity);
}
}