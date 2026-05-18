#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;


public static class DefaultUpgradesCreator
{
    private const string OutputFolder = "Assets/Data/Upgrades";

    [MenuItem("Tools/Shop/Create Default Upgrades")]
    public static void CreateAll()
    {
        EnsureFolder(OutputFolder);

        CreateUpgrade_Vida();
        CreateUpgrade_Dano();
        CreateUpgrade_Mochila();
        CreateUpgrade_Oxigenio();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[Shop] Quatro upgrades iniciais criados em: {OutputFolder}");
        EditorUtility.FocusProjectWindow();
    }

    private static void CreateUpgrade_Vida()
    {
        var data = ScriptableObject.CreateInstance<UpgradeData>();

        data.upgradeName = "Vida";
        data.description = "Aumenta o HP máximo do personagem. Essencial para sobreviver mais tempo em campo.";
        data.baseValue = 20f;   // +20 HP no nível 1
        data.valuePerLevel = 10f;   // +10 HP por nível adicional
        data.maxLevel = 5;

        data.baseCosts = new System.Collections.Generic.List<ResourceCost>
        {
            new ResourceCost { resourceType = ResourceType.Metal, amount = 15 }
        };
        data.costScalingFactor = 1.5f;

        data.spawnWeight = 1.2f;
        data.category = UpgradeCategory.Survival;

        Save(data, "Upgrade_Vida");
    }

    private static void CreateUpgrade_Dano()
    {
        var data = ScriptableObject.CreateInstance<UpgradeData>();

        data.upgradeName = "Dano";
        data.description = "Aumenta o dano base de todos os ataques. Mais letal a cada nível.";
        data.baseValue = 5f;    // +5 dano no nível 1
        data.valuePerLevel = 3f;    // +3 dano por nível adicional
        data.maxLevel = 5;

        data.baseCosts = new System.Collections.Generic.List<ResourceCost>
        {
            new ResourceCost { resourceType = ResourceType.Metal,   amount = 20 },
            new ResourceCost { resourceType = ResourceType.Plastic, amount = 5  }
        };
        data.costScalingFactor = 1.6f;

        data.spawnWeight = 1.0f;
        data.category = UpgradeCategory.Combat;

        Save(data, "Upgrade_Dano");
    }

    private static void CreateUpgrade_Mochila()
    {
        var data = ScriptableObject.CreateInstance<UpgradeData>();

        data.upgradeName = "Mochila";
        data.description = "Aumenta o inventário. Permite carregar mais itens e recursos por expedição.";
        data.baseValue = 5f;    // +5 slots no nível 1
        data.valuePerLevel = 3f;    // +3 slots por nível adicional
        data.maxLevel = 4;

        data.baseCosts = new System.Collections.Generic.List<ResourceCost>
        {
            new ResourceCost { resourceType = ResourceType.Metal,   amount = 10 },
            new ResourceCost { resourceType = ResourceType.Glass, amount = 8  }
        };
        data.costScalingFactor = 1.4f;

        data.spawnWeight = 0.8f;
        data.category = UpgradeCategory.Utility;

        Save(data, "Upgrade_Mochila");
    }

    private static void CreateUpgrade_Oxigenio()
    {
        var data = ScriptableObject.CreateInstance<UpgradeData>();

        data.upgradeName = "Oxigênio";
        data.description = "Aumenta a capacidade do tanque de oxigênio. Permite explorar áreas hostis por mais tempo.";
        data.baseValue = 30f;   // +30 segundos no nível 1
        data.valuePerLevel = 15f;   // +15 segundos por nível adicional
        data.maxLevel = 4;

        data.baseCosts = new System.Collections.Generic.List<ResourceCost>
        {
            new ResourceCost { resourceType = ResourceType.Plastic, amount = 12 },
            new ResourceCost { resourceType = ResourceType.Glass,      amount = 5  }
        };
        data.costScalingFactor = 1.5f;

        data.spawnWeight = 0.9f;
        data.category = UpgradeCategory.Resource;

        Save(data, "Upgrade_Oxigenio");
    }

    private static void Save(UpgradeData data, string assetName)
    {
        string path = $"{OutputFolder}/{assetName}.asset";

        // Não sobrescreve se já existir
        if (File.Exists(Path.Combine(Application.dataPath, path.Replace("Assets/", ""))))
        {
            Debug.LogWarning($"[Shop] Asset já existe, pulando: {path}");
            return;
        }

        AssetDatabase.CreateAsset(data, path);
        Debug.Log($"[Shop] Criado: {path}");
    }

    private static void EnsureFolder(string folderPath)
    {
        string[] parts = folderPath.Split('/');
        string current = parts[0]; // "Assets"

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}
#endif