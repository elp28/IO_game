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

    [Header("Custo — Slot 1 (sempre visível)")]
    [SerializeField] private Image iconCostLine1;
    [SerializeField] private TextMeshProUGUI valueCostLine1;

    [Header("Custo — Slot 2 (visível só se tiver 2 recursos)")]
    [SerializeField] private GameObject valueForBuy2;
    [SerializeField] private Image iconCostLine2;
    [SerializeField] private TextMeshProUGUI valueCostLine2;

    [Header("Botão de Compra")]
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;

    [Header("Banco de Ícones")]
    [SerializeField] private ResourceIconDatabase iconDatabase;

    [Header("Tamanho máximo do ícone (mantém proporção da sprite)")]
    [SerializeField] private float maxUpgradeIconSize = 64f;
    [SerializeField] private float maxCostIconSize = 32f;

    private ShopOffer _offer;

    public void Setup(ShopOffer offer)
    {
        _offer = offer;

        if (upgradeIcon != null) SetIconWithNativeAspect(upgradeIcon, offer.Icon, maxUpgradeIconSize);
        if (nameText != null) nameText.text = offer.DisplayName;
        if (descriptionText != null) descriptionText.text = offer.Description;
        if (levelText != null) levelText.text = $"Nível {offer.CurrentLevel + 1} → {offer.NextLevel + 1}";
        if (valueText != null) valueText.text = FormatValue(offer);

        BuildCosts(offer);
        RefreshButton();
    }

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

    public void OnBuyButtonClicked()
    {
        if (_offer == null) return;

        bool success = ShopManager.instance.TryPurchase(_offer);

        if (success)
            ShopCanvasController.instance.OnPurchaseCompleted();
        else
            RefreshButton();
    }

    private void BuildCosts(ShopOffer offer)
    {
        List<ResourceCost> costs = offer.Cost;

        if (costs.Count >= 1)
        {
            SetIconWithNativeAspect(iconCostLine1, GetCostIcon(offer, 0), maxCostIconSize);
            if (valueCostLine1 != null) valueCostLine1.text = costs[0].amount.ToString();
        }

        bool hasSecond = costs.Count >= 2;
        if (valueForBuy2 != null) valueForBuy2.SetActive(hasSecond);

        if (hasSecond)
        {
            SetIconWithNativeAspect(iconCostLine2, GetCostIcon(offer, 1), maxCostIconSize);
            if (valueCostLine2 != null) valueCostLine2.text = costs[1].amount.ToString();
        }
    }

    /// <summary>
    /// Define a sprite no Image e ajusta o RectTransform pra respeitar a
    /// proporção real (largura/altura) da sprite, encaixando dentro de um
    /// tamanho máximo (comportamento tipo "object-fit: contain").
    /// </summary>
    private void SetIconWithNativeAspect(Image image, Sprite sprite, float maxSize)
    {
        if (image == null) return;

        image.sprite = sprite;

        if (sprite == null) return;

        RectTransform rt = image.rectTransform;
        Rect spriteRect = sprite.rect;

        if (spriteRect.height <= 0f) return;

        float aspect = spriteRect.width / spriteRect.height;

        float width, height;
        if (aspect >= 1f)
        {
            width = maxSize;
            height = maxSize / aspect;
        }
        else
        {
            height = maxSize;
            width = maxSize * aspect;
        }

        rt.sizeDelta = new Vector2(width, height);
    }

    private Sprite GetCostIcon(ShopOffer offer, int index)
    {
        if (index >= offer.Cost.Count) return null;

        ResourceType type = offer.Cost[index].resourceType;

        if (iconDatabase != null) return iconDatabase.GetIcon(type);
        if (ResourceIconDatabase.instance != null) return ResourceIconDatabase.instance.GetIcon(type);

        return null;
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