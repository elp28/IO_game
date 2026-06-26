using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostLineUI : MonoBehaviour
{
    [Header("Referências — arraste no Inspector do prefab")]
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("Ícones por Tipo de Recurso")]
    [SerializeField] private Sprite iconPlastic;
    [SerializeField] private Sprite iconMetal;
    [SerializeField] private Sprite iconGlass;

    public void Setup(ResourceType type, int amount)
    {
        Debug.Log($"[CostLine] Setup chamado — tipo: {type} | amount: {amount} | resourceIcon: {resourceIcon} | amountText: {amountText}");

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