using UnityEngine;
using DG.Tweening;

public class PulseAnimation : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float pulseScale = 1.15f;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Ease easeType = Ease.InOutSine;

    [Header("Desaparecer")]
    [SerializeField] private float disappearDuration = 0.3f;

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
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Disappear()
    {
        transform.DOKill();

        transform.DOScale(Vector3.zero, disappearDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}