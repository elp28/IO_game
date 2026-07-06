using UnityEngine;
using DG.Tweening;

/// <summary>
/// Ícone visual que representa um recurso saindo da mochila.
/// Spawna no player, vai até a estação e some.
/// </summary>
public class ResourceIconVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 8f;

    private System.Action _onArrived;

    /// <summary>
    /// Configura o ícone e começa o movimento até a estação.
    /// </summary>
    public void Setup(Sprite sprite, Transform station, System.Action onArrived)
    {
        if (spriteRenderer != null) spriteRenderer.sprite = sprite;
        _onArrived = onArrived;

        // Guarda a escala original do prefab e anima até ela
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack);

        // Vai até a estação
        float distance = Vector3.Distance(transform.position, station.position);
        float duration = distance / moveSpeed;

        transform.DOMove(station.position, duration)
            .SetEase(Ease.InQuad)
            .OnComplete(OnArrived);
    }

    private void OnArrived()
    {
        _onArrived?.Invoke();
        transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => Destroy(gameObject));
    }
}