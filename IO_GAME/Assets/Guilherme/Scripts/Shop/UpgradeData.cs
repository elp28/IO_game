using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Shop/Upgrade Data", order = 0)]
public class UpgradeData : ScriptableObject
{
    [Header("Visual")]
    public string upgradeName = "Upgrade";

    [TextArea(2, 4)]
    public string description = "";

    [ShowAssetPreview(64, 64)]
    public Sprite icon;

    [Header("Progressão")]
    public float baseValue = 1f;
    public float valuePerLevel = 0.5f;

    [Min(0)]
    public int maxLevel = 5;

    [Header("Custo")]
    [Tooltip("Máximo 2 recursos.")]
    public List<ResourceCost> baseCosts = new List<ResourceCost>();

    [Min(1f)]
    public float costScalingFactor = 1.5f;

    [Header("Custo — Ícones")]
    [Tooltip("Ícone do primeiro recurso. Sempre preenchido.")]
    [ShowAssetPreview(64, 64)]
    public Sprite costIcon1;

    [Tooltip("Ícone do segundo recurso. Deixe vazio se o upgrade tiver só 1 recurso.")]
    [ShowAssetPreview(64, 64)]
    public Sprite costIcon2;

    [Header("Geração Procedural")]
    [Min(0f)]
    public float spawnWeight = 1f;

    public UpgradeCategory category = UpgradeCategory.Combat;

    // ─────────────────────────────────────────────
    // MÉTODOS
    // ─────────────────────────────────────────────

    public float GetValueAtLevel(int level)
    {
        if (level <= 0) return 0f;
        return baseValue + valuePerLevel * (level - 1);
    }

    public List<ResourceCost> GetCostsAtLevel(int level)
    {
        if (level <= 0) return new List<ResourceCost>();

        var scaledCosts = new List<ResourceCost>();
        float multiplier = Mathf.Pow(costScalingFactor, level - 1);

        foreach (var cost in baseCosts)
        {
            scaledCosts.Add(new ResourceCost
            {
                resourceType = cost.resourceType,
                amount = Mathf.RoundToInt(cost.amount * multiplier)
            });
        }

        return scaledCosts;
    }

    public bool IsMaxLevel(int level)
    {
        return maxLevel > 0 && level >= maxLevel;
    }
}

[Serializable]
public class ResourceCost
{
    public ResourceType resourceType;

    [Min(1)]
    public int amount = 10;
}

public enum ResourceType
{
    Metal,
    Plastic,
    Glass,
}

public enum UpgradeCategory
{
    Combat,
    Survival,
    Utility,
    Resource
}