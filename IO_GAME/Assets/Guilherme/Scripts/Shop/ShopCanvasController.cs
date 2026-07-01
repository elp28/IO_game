using System.Collections.Generic;
using UnityEngine;

public class ShopCanvasController : MonoBehaviour
{
    public static ShopCanvasController instance;

    [Header("Referências")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform  offersContainer;
    [SerializeField] private GameObject shopOfferUIPrefab;

    // Estação que abriu a loja atualmente
    private ShopStation _currentStation;

    void Awake()
    {
        if (instance == null) instance = this;
        CloseShop();
    }

    /// <summary>
    /// Abre a loja para uma estação específica.
    /// Chamado pela estação quando o jogador interage com ela.
    /// </summary>
    public void OpenShop(ShopStation station)
    {
        _currentStation = station;

        List<ShopOffer> offers = station.GetOffers();
        BuildBanners(offers);

        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearBanners();
        _currentStation = null;
    }

    /// <summary>
    /// Chamado pelo ShopOfferUI após uma compra bem-sucedida.
    /// Avisa a estação que precisa regenerar as ofertas na próxima abertura.
    /// </summary>
    public void OnPurchaseCompleted()
    {
        if (_currentStation != null)
            _currentStation.OnPurchaseCompleted();

        CloseShop();
    }

    private void BuildBanners(List<ShopOffer> offers)
    {
        ClearBanners();

        foreach (var offer in offers)
        {
            GameObject obj     = Instantiate(shopOfferUIPrefab, offersContainer);
            ShopOfferUI banner = obj.GetComponent<ShopOfferUI>();
            if (banner != null)
                banner.Setup(offer);
        }
    }

    private void ClearBanners()
    {
        if (offersContainer == null) return;

        foreach (Transform child in offersContainer)
            Destroy(child.gameObject);
    }
}