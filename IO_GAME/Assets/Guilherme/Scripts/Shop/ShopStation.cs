using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coloque este componente em cada estação da cena.
/// Cada estação guarda suas próprias ofertas independentemente.
/// Ofertas são regeneradas apenas após uma compra.
/// </summary>
public class ShopStation : MonoBehaviour
{
    // Ofertas desta estação — persistem até uma compra ser feita
    private List<ShopOffer> _offers = null;

    // Flag: indica que uma compra foi feita e as ofertas devem ser regeneradas
    private bool _needsRegeneration = true;

    /// <summary>
    /// Retorna as ofertas desta estação.
    /// Gera novas apenas se necessário.
    /// </summary>
    public List<ShopOffer> GetOffers()
    {
        if (_needsRegeneration || _offers == null)
        {
            _offers = ShopManager.instance.GenerateOffers();
            _needsRegeneration = false;
            Debug.Log($"[{gameObject.name}] Novas ofertas geradas.");
        }
        else
        {
            Debug.Log($"[{gameObject.name}] Reutilizando ofertas anteriores.");
        }

        return _offers;
    }

    /// <summary>
    /// Chamado pelo ShopCanvasController após uma compra bem-sucedida.
    /// </summary>
    public void OnPurchaseCompleted()
    {
        _needsRegeneration = true;
    }
}