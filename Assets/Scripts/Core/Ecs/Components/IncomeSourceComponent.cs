namespace Core.Ecs.Components
{
internal struct IncomeSourceComponent
{
    public double BaseIncomeAmount;
    public double IncomeMultiplier;
    public int IncomeSrcLvl;

    public double TotalIncomeAmount => BusinessFormulas.BusinessTotalIncome(
        BaseIncomeAmount, IncomeMultiplier, IncomeSrcLvl);
}
}