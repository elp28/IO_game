using System.Collections.Generic;
using UnityEngine;

public class ShopCanvasController : MonoBehaviour
{
    public static ShopCanvasController instance;

    [Header("Referências")]
    [Tooltip("O painel raiz da loja (ShopCanvas ou um painel filho).")]
    [SerializeField] private GameObject shopPanel;

    [Tooltip("Container onde os banners ShopOfferUI serão instanciados.")]
    [SerializeField] private Transform offersContainer;

    [Tooltip("Prefab do banner de oferta.")]
    [SerializeField] private GameObject shopOfferUIPrefab;


    void Awake()
    {
        if (instance == null) instance = this;
        CloseShop(); // garante que começa fechado
    }

    public void OpenShop()
    {
        List<ShopOffer> offers = ShopManager.instance.OpenShop();

        BuildBanners(offers);

        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearBanners();
    }


    public void OnPurchaseCompleted()
    {
        CloseShop();
    }

    private void BuildBanners(List<ShopOffer> offers)
    {
        ClearBanners();

        foreach (var offer in offers)
        {
            GameObject obj = Instantiate(shopOfferUIPrefab, offersContainer);
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