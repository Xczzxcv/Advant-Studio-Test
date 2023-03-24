namespace Core
{
internal static class BusinessFormulas
{
    public static double BusinessTotalIncome(double baseIncomeAmount, double businessIncomeMultiplier, 
        int businessLvl)
    {
        return baseIncomeAmount * businessLvl * businessIncomeMultiplier;
    }
    
    public static double NextBusinessLvlCostFormula(int currentBusinessLvl, double baseBusinessPrice)
    {
        return (currentBusinessLvl + 1) * baseBusinessPrice;
    }
}
}