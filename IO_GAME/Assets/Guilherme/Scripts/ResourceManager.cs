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
        totalGlass += glass;
        totalPlastic += plastic;
        totalMetal += metal;

        Debug.Log($"[BASE] Processamento concluído! Vidro: {totalGlass} | Plástico: {totalPlastic} | Metal: {totalMetal}");

        hudManager.UpdateResources(totalGlass, totalPlastic, totalMetal);
    }



   
}