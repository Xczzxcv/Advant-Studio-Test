namespace Core.Ecs.Components
{
internal struct ProgressComponent
{
    public double ProgressAmount;
    public double ProgressPeriod;
    public bool IsActive;

    public bool IsCompleted => ProgressAmount >= 1;
}
}