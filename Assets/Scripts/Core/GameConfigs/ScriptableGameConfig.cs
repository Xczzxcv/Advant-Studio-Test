using System.Diagnostics;
using UnityEngine;

namespace Core.GameConfigs
{
internal abstract class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [SerializeField] private string id;
    
    public string Id => id;

    [Conditional("UNITY_EDITOR")]
    internal void SetId_EditorOnly(string newId)
    {
        id = newId;
    }
}
}