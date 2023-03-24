using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
internal class GameUpdater : MonoBehaviour
{
    private readonly List<IUpdatable> _updatables = new();

    public void Add(params IUpdatable[] updatable)
    {
        _updatables.AddRange(updatable);
    }

    private void Update()
    {
        foreach (var updatable in _updatables)
        {
            updatable.OnUpdate();
        }
    }
}
}