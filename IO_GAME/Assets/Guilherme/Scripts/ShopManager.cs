using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cérebro da loja. Gera ofertas, processa compras.
/// Não conhece a UI — entrega uma lista de ShopOffer e para por aí.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("Configuração")]
    [Tooltip("Todos os upgrades disponíveis no jogo. Arraste os assets aqui.")]
    [SerializeField] private List<UpgradeData> allUpgrades = new List<UpgradeData>();

    [Tooltip("Quantas ofertas são geradas por abertura da loja.")]
    [SerializeField] private int offersPerOpen = 3;

    // ─────────────────────────────────────────────
    // ESTADO
    // ─────────────────────────────────────────────

    /// <summary>Ofertas da rodada atual. Recriadas toda vez que a loja abre.</summary>
    public List<ShopOffer> CurrentOffers { get; private set; } = new List<ShopOffer>();

    // ─────────────────────────────────────────────
    // LIFECYCLE
    // ─────────────────────────────────────────────

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // ─────────────────────────────────────────────
    // API PÚBLICA
    // ─────────────────────────────────────────────

    /// <summary>
    /// Abre a loja: gera novas ofertas e retorna a lista.
    /// Chamar sempre que o painel da loja for aberto.
    /// </summary>
    public List<ShopOffer> OpenShop()
    {
        CurrentOffers = GenerateOffers();

        Debug.Log($"[Loja] {CurrentOffers.Count} oferta(s) gerada(s).");
        foreach (var o in CurrentOffers)
            Debug.Log($"  → {o.DisplayName} (Nível {o.CurrentLevel} → {o.NextLevel}) | Valor: {o.ValueAfterPurchase}");

        return CurrentOffers;
    }

    /// <summary>
    /// Tenta comprar uma oferta.
    /// Retorna true se a compra foi concluída, false se o jogador não puder pagar.
    /// </summary>
    public bool TryPurchase(ShopOffer offer)
    {
        if (offer == null || offer.IsMaxLevel)
        {
            Debug.LogWarning("[Loja] Oferta inválida ou upgrade já no nível máximo.");
            return false;
        }

        if (!ResourceManager.instance.CanAfford(offer.Cost))
        {
            Debug.Log($"[Loja] Recursos insuficientes para: {offer.DisplayName}");
            return false;
        }

        ResourceManager.instance.Spend(offer.Cost);
        PlayerUpgradeManager.instance.ApplyUpgrade(offer.Upgrade);

        Debug.Log($"[Loja] Compra concluída: {offer.DisplayName} → Nível {offer.NextLevel}");

        CurrentOffers.Remove(offer);
        return true;
    }

    // ─────────────────────────────────────────────
    // GERAÇÃO DE OFERTAS
    // ─────────────────────────────────────────────

    public List<ShopOffer> GenerateOffers()
    {
        // 1. Filtrar upgrades inválidos
        var valid = FilterValid();

        if (valid.Count == 0)
        {
            Debug.Log("[Loja] Nenhum upgrade disponível.");
            return new List<ShopOffer>();
        }

        // 2. Sortear por peso
        int count = Mathf.Min(offersPerOpen, valid.Count);
        var selected = WeightedSample(valid, count);

        // 3. Criar as ShopOffers
        var offers = new List<ShopOffer>();
        foreach (var upgrade in selected)
        {
            int currentLevel = PlayerUpgradeManager.instance.GetLevel(upgrade);
            offers.Add(new ShopOffer(upgrade, currentLevel));
        }

        return offers;
    }

    /// <summary>
    /// Remove upgrades que não devem aparecer na loja:
    /// - já estão no nível máximo
    /// - peso zero (desativados)
    /// </summary>
    private List<UpgradeData> FilterValid()
    {
        var valid = new List<UpgradeData>();

        foreach (var upgrade in allUpgrades)
        {
            if (upgrade == null)             continue;
            if (upgrade.spawnWeight <= 0f)   continue;

            int currentLevel = PlayerUpgradeManager.instance.GetLevel(upgrade);
            if (upgrade.IsMaxLevel(currentLevel)) continue;

            valid.Add(upgrade);
        }

        return valid;
    }

    /// <summary>
    /// Sorteia <count> upgrades únicos da lista usando peso relativo.
    /// </summary>
    private List<UpgradeData> WeightedSample(List<UpgradeData> pool, int count)
    {
        var remaining = new List<UpgradeData>(pool);
        var result    = new List<UpgradeData>();

        for (int i = 0; i < count && remaining.Count > 0; i++)
        {
            float totalWeight = 0f;
            foreach (var u in remaining) totalWeight += u.spawnWeight;

            float roll = Random.Range(0f, totalWeight);
            float accumulated = 0f;

            for (int j = 0; j < remaining.Count; j++)
            {
                accumulated += remaining[j].spawnWeight;
                if (roll <= accumulated)
                {
                    result.Add(remaining[j]);
                    remaining.RemoveAt(j);   // garante que não repete
                    break;
                }
            }
        }

        return result;
    }
}