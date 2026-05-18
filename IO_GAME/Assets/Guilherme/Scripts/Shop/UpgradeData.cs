using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Shop/Upgrade Data", order = 0)]
public class UpgradeData : ScriptableObject
{


    [Header("Visual")]
    [Tooltip("Nome exibido na loja e no HUD.")]
    public string upgradeName = "Upgrade";

    [Tooltip("Descrição curta exibida na loja.")]
    [TextArea(2, 4)]
    public string description = "";

    [Tooltip("Ícone exibido na UI.")]
    public Sprite icon;

    [Header("Progressão")]
    [Tooltip("Valor do efeito no nível 1.")]
    public float baseValue = 1f;

    [Tooltip("Quanto o valor cresce por nível.")]
    public float valuePerLevel = 0.5f;

    [Tooltip("Nível máximo atingível. Use 0 para ilimitado.")]
    [Min(0)]
    public int maxLevel = 5;

    [Header("Custo")]
    [Tooltip("Lista de recursos necessários para comprar o nível 1.")]
    public List<ResourceCost> baseCosts = new List<ResourceCost>();

    [Tooltip("Fator de escalonamento do custo por nível. Ex: 1.5 = 50% mais caro por nível.")]
    [Min(1f)]
    public float costScalingFactor = 1.5f;


    [Header("Geração Procedural")]
    [Tooltip("Peso relativo de aparição na loja. Valores maiores = aparece mais.")]
    [Min(0f)]
    public float spawnWeight = 1f;

    [Tooltip("Categoria do upgrade para filtragem e agrupamento.")]
    public UpgradeCategory category = UpgradeCategory.Combat;

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
    [Tooltip("Tipo do recurso exigido.")]
    public ResourceType resourceType;

    [Tooltip("Quantidade base necessária.")]
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
    Combat,     // Dano, crítico, velocidade de ataque
    Survival,   // Vida, escudo, regeneração
    Utility,    // Mochila, cooldowns, velocidade
    Resource    // Oxigênio, geração de recursos
}