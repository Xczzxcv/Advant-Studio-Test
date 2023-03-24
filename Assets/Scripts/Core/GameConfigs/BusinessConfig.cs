using UnityEngine;

namespace Core.GameConfigs
{
[CreateAssetMenu(menuName = "Business Config", fileName = "BusinessConfig", order = 0)]
internal class BusinessConfig : ScriptableGameConfig
{
    public double ProgressPeriod;
    public double BaseIncomeAmount;
    public double BasePriceAmount;
    public BusinessUpgrade[] Upgrades;
    public bool IsInitial;
}
}