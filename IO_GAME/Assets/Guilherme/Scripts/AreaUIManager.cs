using UnityEngine;
using TMPro;

public class AreaUIManager : MonoBehaviour
{
    public static AreaUIManager instance;

    [Header("Referências")]
    [SerializeField] private GameObject areaPanel;
    [SerializeField] private GameObject enemyCount;      // pai com icon + text
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI clearedText;

    void Awake()
    {
        instance = this;
        areaPanel.SetActive(false);
        if (enemyCount  != null) enemyCount.SetActive(false);
        if (clearedText != null) clearedText.gameObject.SetActive(false);
    }

    public void ShowArea(PollutedArea area)
    {
        areaPanel.SetActive(true);

        if (area.IsCleared)
        {
            if (enemyCount  != null) enemyCount.SetActive(false);
            if (clearedText != null)
            {
                clearedText.text = "Área Limpa!";
                clearedText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (clearedText != null) clearedText.gameObject.SetActive(false);
            if (enemyCount  != null) enemyCount.SetActive(true);
            UpdateEnemyCount(area);
        }
    }

    public void HideArea()
    {
        areaPanel.SetActive(false);
    }

    public void UpdateEnemyCount(PollutedArea area)
    {
        if (enemyCountText != null)
            enemyCountText.text = area.EnemyCount.ToString();
    }

    public void OnAreaCleared(PollutedArea area)
    {
        areaPanel.SetActive(true);
        if (enemyCount  != null) enemyCount.SetActive(false);
        if (clearedText != null)
        {
            clearedText.text = "Área Limpa!";
            clearedText.gameObject.SetActive(true);
        }
    }
}