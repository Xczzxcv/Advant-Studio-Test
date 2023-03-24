using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
// #if UNITY_EDITOR
using UnityEditor;
// #endif
using UnityEngine;

namespace Core.GameConfigs
{
#if UNITY_EDITOR
internal partial class GameConfigsProvider
{
    [SerializeField] private string viewConfigPostfix;

    [ContextMenu("Update Config Ids")]
    private void UpdateConfigIds()
    {
        var configsToUpdate = Enumerable.Empty<ScriptableGameConfig>()
            .Concat(businesses)
            .Concat(GetBusinessUpgradesEnumerable())
            .Concat(businessViews)
            .Concat(businessUpgradeViews);
        
        foreach (var gameConfig in configsToUpdate)
        {
            UpdateGameConfigId(gameConfig);
        }
    }

    private void UpdateGameConfigId(ScriptableGameConfig gameConfig)
    {
        var gameConfigAssetPath = AssetDatabase.GetAssetPath(gameConfig);
        var configId = Path.GetFileNameWithoutExtension(gameConfigAssetPath);
        configId = configId.TrimEnd(viewConfigPostfix);
        if (configId == gameConfig.Id)
        {
            return;
        }

        gameConfig.SetId_EditorOnly(configId);
        EditorUtility.SetDirty(gameConfig);
        AssetDatabase.SaveAssetIfDirty(gameConfig);
        AssetDatabase.Refresh();
    }

    private IEnumerable<BusinessUpgrade> GetBusinessUpgradesEnumerable()
    {
        return businesses
            .Where(businessConfig => businessConfig != null)
            .Select(businessConfig => businessConfig.Upgrades)
            .SelectMany(upgrades => upgrades);
    }
}
#endif
}