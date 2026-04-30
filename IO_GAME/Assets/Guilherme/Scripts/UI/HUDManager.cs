using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    [Header("Referências do Player")]
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private PlayerCollect playerCollect;

    [Header("Barra de Vida")]
    [SerializeField] private Image lifeBarFill;
    [SerializeField] private float lifeAnimDuration = 0.3f;

    [Header("Barra de Oxigênio")]
    [SerializeField] private Image oxyBarFill;

    [Header("Mochila")]
    [SerializeField] private TextMeshProUGUI bagText;

    private readonly Color oxyColorFull     = new Color(0.2f, 0.3f, 0.8f);
    private readonly Color colorFull     = new Color(0.2f, 0.8f, 0.3f);
    private readonly Color oxyColorMid      = new Color(0.5f,   0.4f, 0.7f);
    private readonly Color colorMid      = new Color(1f,   0.7f, 0f);
    private readonly Color oxyColorCritical = new Color(0.9f, 0.1f, 0.2f);
    private readonly Color colorCritical = new Color(0.9f, 0.2f, 0.2f);

    private float lastLifePercent = 1f;
    private int lastBagTotal = 0;
    float lastOxygen;

    void Update()
    {
        RefreshLife();
        RefreshBag();
        RefreshOxygen();
    }

    void Start()
    {
        lastBagTotal = -1; // força atualização inicial
        RefreshBag();
    }

    void RefreshLife()
    {
        if (playerLife == null || lifeBarFill == null) return;

        float percent = playerLife.LifePercent;

        if (Mathf.Approximately(percent, lastLifePercent)) return;
        lastLifePercent = percent;

        lifeBarFill.DOFillAmount(percent, lifeAnimDuration).SetEase(Ease.OutCubic);

        Color targetColor = percent > 0.6f ? colorFull :
                            percent > 0.3f ? colorMid  : colorCritical;
        lifeBarFill.DOColor(targetColor, lifeAnimDuration);

        if (percent <= 0.3f)
            lifeBarFill.rectTransform.DOShakePosition(0.3f, strength: 6f, vibrato: 20);
    }

    void RefreshBag()
    {
        if (playerCollect == null || bagText == null) return;

        int current = playerCollect.CurrentTotal;
        int max     = playerCollect.MaxCapacity;

        if (current == lastBagTotal) return;
        lastBagTotal = current;

        bagText.text = $"{current}/{max}";

        bagText.rectTransform.DOKill();
        bagText.rectTransform.localScale = Vector3.one;
        bagText.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.25f, vibrato: 6);

        bagText.color = (current >= max) ? new Color(1f, 0.5f, 0f) : Color.white;
    }

    void RefreshOxygen()
    {
        if (playerLife == null || oxyBarFill == null) return;

        float percent = playerLife.oxygenPercent;

        if (Mathf.Approximately(percent, lastOxygen)) return;
        lastOxygen = percent;

        oxyBarFill.DOFillAmount(percent, 0.1f).SetEase(Ease.OutCubic);

        Color targetColor = percent > 0.6f ? oxyColorFull :
                            percent > 0.3f ? oxyColorMid  : oxyColorCritical;
        oxyBarFill.DOColor(targetColor, 0.1f);

        if (percent <= 0.3f)
            oxyBarFill.rectTransform.DOShakePosition(0.3f, strength: 1.5f, vibrato: 10);
    }
}