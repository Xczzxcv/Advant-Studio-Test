namespace Infrastructure
{
internal interface IBusinessLevelIncreaser
{
    bool CanIncreaseBusinessLevel(string businessId);
    void IncreaseBusinessLevel(string businessId, bool isFree = false);
    void PurchaseBusiness(string businessId);
}
}