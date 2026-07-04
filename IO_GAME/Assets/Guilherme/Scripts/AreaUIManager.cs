using UnityEngine;
using TMPro;

public class AreaUIManager : MonoBehaviour
{
    public static AreaUIManager instance;

    [Header("Referências")]
    [SerializeField] private GameObject areaPanel;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI clearedText;

    void Awake()
    {
        instance = this;
        areaPanel.SetActive(false);
        if (clearedText != null) clearedText.gameObject.SetActive(false);
    }

    public void ShowArea(PollutedArea area)
    {
        areaPanel.SetActive(true);

        if (area.IsCleared)
        {
            // Área já foi limpa — mostra só o texto de limpa
            if (enemyCountText != null) enemyCountText.gameObject.SetActive(false);
            if (clearedText    != null)
            {
                clearedText.text = "Área Limpa!";
                clearedText.gameObject.SetActive(true);
            }
        }
        else
        {
            // Área ainda tem inimigos
            if (clearedText    != null) clearedText.gameObject.SetActive(false);
            if (enemyCountText != null) enemyCountText.gameObject.SetActive(true);
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
        if (enemyCountText != null) enemyCountText.gameObject.SetActive(false);
        if (clearedText    != null)
        {
            clearedText.text = "Área Limpa!";
            clearedText.gameObject.SetActive(true);
        }
    }
}