using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    [Header("Banco de Materiais (Pós-Processamento)")]
    public int totalGlass;
    public int totalPlastic;
    public int totalMetal;

    [Header("HudManager")]
    [SerializeField] HUDManager hudManager;


    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        hudManager.UpdateResources(totalGlass, totalPlastic, totalMetal);
    }


    public void ConvertTrashToResource(int glass, int plastic, int metal)
    {
        totalGlass   += glass;
        totalPlastic += plastic;
        totalMetal   += metal;

        Debug.Log($"[BASE] Processamento concluído! Vidro: {totalGlass} | Plástico: {totalPlastic} | Metal: {totalMetal}");

        hudManager.UpdateResources(totalGlass, totalPlastic, totalMetal);
    }

    public int GetAmount(ResourceType type)
    {
        return type switch
        {
            ResourceType.Glass   => totalGlass,
            ResourceType.Plastic => totalPlastic,
            ResourceType.Metal   => totalMetal,
            _                    => 0
        };
    }

    public bool CanAfford(List<ResourceCost> costs)
    {
        foreach (var cost in costs)
        {
            if (GetAmount(cost.resourceType) < cost.amount)
                return false;
        }
        return true;
    }

    public void Spend(List<ResourceCost> costs)
    {
        foreach (var cost in costs)
        {
            switch (cost.resourceType)
            {
                case ResourceType.Glass:   totalGlass   -= cost.amount; break;
                case ResourceType.Plastic: totalPlastic -= cost.amount; break;
                case ResourceType.Metal:   totalMetal   -= cost.amount; break;
            }
        }

        Debug.Log($"[LOJA] Compra deduzida. Vidro: {totalGlass} | Plástico: {totalPlastic} | Metal: {totalMetal}");

        hudManager.UpdateResources(totalGlass, totalPlastic, totalMetal);
    }
}