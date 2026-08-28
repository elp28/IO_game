using UnityEngine;
using DG.Tweening;

public class PulseAnimation : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float pulseScale = 1.15f; // quanto ele cresce (1.15 = 15% maior)
    [SerializeField] private float duration = 0.6f;     // tempo de cada ida (cresce ou diminui)
    [SerializeField] private Ease easeType = Ease.InOutSine;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.DOKill();
        transform.localScale = originalScale;

        transform.DOScale(originalScale * pulseScale, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo); // -1 = infinito, Yoyo = vai e volta
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}