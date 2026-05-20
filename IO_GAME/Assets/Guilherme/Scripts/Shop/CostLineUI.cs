using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostLineUI : MonoBehaviour
{
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("Ícones por Tipo de Recurso")]
    [SerializeField] private Sprite iconPlastic;
    [SerializeField] private Sprite iconMetal;
    [SerializeField] private Sprite iconGlass;

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