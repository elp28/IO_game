using UnityEngine;
using DG.Tweening;

public class AppearAnimation : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private Ease easeType = Ease.OutBack;

    private CanvasGroup canvasGroup;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    private void Awake()
    {
        // Guarda a escala original (a que você configurou no Inspector)
        originalScale = transform.localScale;

        // Tenta pegar componentes de fade, se existirem
        canvasGroup = GetComponent<CanvasGroup>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Mata qualquer tween anterior nesse transform (evita conflito)
        transform.DOKill();

        // Começa em zero e cresce até o tamanho original
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, duration).SetEase(easeType);

        // Fade opcional, dependendo do que o objeto tiver
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, duration);
        }
        else if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;
            spriteRenderer.DOFade(1f, duration);
        }
    }
}