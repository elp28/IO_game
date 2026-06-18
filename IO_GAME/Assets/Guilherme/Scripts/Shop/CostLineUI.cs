using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostLineUI : MonoBehaviour
{
    [Header("Ícones por Tipo de Recurso")]
    [SerializeField] private Sprite iconPlastic;
    [SerializeField] private Sprite iconMetal;
    [SerializeField] private Sprite iconGlass;

    private Image resourceIcon;
    private TextMeshProUGUI amountText;

    void Awake()
    {
        resourceIcon = transform.Find("IconCostLine").GetComponent<Image>();
        amountText = transform.Find("ValueCostLIne").GetComponent<TextMeshProUGUI>();
    }

    public void Setup(ResourceType type, int amount)
    {
        if (amountText != null) amountText.text = amount.ToString();
        if (resourceIcon != null) resourceIcon.sprite = GetIcon(type);
    }

    private Sprite GetIcon(ResourceType type)
    {
        return type switch
        {
            ResourceType.Plastic => iconPlastic,
            ResourceType.Metal => iconMetal,
            ResourceType.Glass => iconGlass,
            _ => null
        };
    }
}