using UnityEngine;

namespace Infrastructure
{
internal class EnvironmentServices
{
    public double CurrentTime => Time.timeAsDouble;
    public double DeltaTime => Time.deltaTime;
}
}