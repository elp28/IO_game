using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopOfferUI : MonoBehaviour
{
    [Header("Visual — Upgrade")]
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI valueText;

    [Header("Visual — Custo")]
    [Tooltip("Container onde as linhas de custo serão instanciadas.")]
    [SerializeField] private Transform costContainer;
    [Tooltip("Prefab de uma linha de custo (ícone + quantidade).")]
    [SerializeField] private GameObject costLinePrefab;

    [Header("Botão de Compra")]
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;

    // ─────────────────────────────────────────────
    // ESTADO
    // ─────────────────────────────────────────────

    private ShopOffer _offer;

    // ─────────────────────────────────────────────
    // API PÚBLICA
    // ─────────────────────────────────────────────

    /// <summary>
    /// Popula o banner com os dados de uma ShopOffer.
    /// Chamado pelo ShopCanvasController ao abrir a loja.
    /// </summary>
    public void Setup(ShopOffer offer)
    {
        _offer = offer;

        // Ícone e textos
        if (upgradeIcon != null) upgradeIcon.sprite = offer.Icon;
        if (nameText != null) nameText.text = offer.DisplayName;
        if (descriptionText != null) descriptionText.text = offer.Description;
        if (levelText != null) levelText.text = $"Nível {offer.CurrentLevel} → {offer.NextLevel}";
        if (valueText != null) valueText.text = FormatValue(offer);

        // Custos
        BuildCostLines(offer.Cost);

        // Botão
        RefreshButton();
    }

    /// <summary>
    /// Atualiza o estado do botão sem recriar o banner inteiro.
    /// Útil para chamar quando os recursos do jogador mudarem.
    /// </summary>
    public void RefreshButton()
    {
        if (buyButton == null || _offer == null) return;

        bool canAfford = ResourceManager.instance.CanAfford(_offer.Cost);
        bool isMax = _offer.IsMaxLevel;

        buyButton.interactable = canAfford && !isMax;

        if (buyButtonText != null)
        {
            if (isMax) buyButtonText.text = "MAX";
            else if (canAfford) buyButtonText.text = "Comprar";
            else buyButtonText.text = "Sem recursos";
        }
    }

    // ─────────────────────────────────────────────
    // EVENTO DO BOTÃO
    // Conecte no onClick do buyButton via Inspector
    // ─────────────────────────────────────────────

    public void OnBuyButtonClicked()
    {
        if (_offer == null) return;

        bool success = ShopManager.instance.TryPurchase(_offer);

        if (success)
            ShopCanvasController.instance.OnPurchaseCompleted();
        else
            RefreshButton();
    }

    // ─────────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────────

    private void BuildCostLines(List<ResourceCost> costs)
    {
        if (costContainer == null || costLinePrefab == null) return;

        foreach (Transform child in costContainer)
            Destroy(child.gameObject);

        foreach (var cost in costs)
        {
            GameObject line = Instantiate(costLinePrefab, costContainer);
            CostLineUI costLine = line.GetComponent<CostLineUI>();
            if (costLine != null)
                costLine.Setup(cost.resourceType, cost.amount);
        }
    }

    private string FormatValue(ShopOffer offer)
    {
        return offer.Upgrade.category switch
        {
            UpgradeCategory.Survival when offer.DisplayName == "Vida" => $"+{offer.ValueAfterPurchase} HP",
            UpgradeCategory.Survival when offer.DisplayName == "Oxigênio" => $"+{offer.ValueAfterPurchase}s O₂",
            UpgradeCategory.Combat => $"+{offer.ValueAfterPurchase} DMG",
            UpgradeCategory.Utility => $"+{offer.ValueAfterPurchase} slots",
            _ => $"+{offer.ValueAfterPurchase}"
        };
    }
}