using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Painéis")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    // Quantas áreas existem na cena
    private int _totalAreas = 0;
    private int _clearedAreas = 0;

    void Awake()
    {
        if (instance == null) instance = this;

        gameOverPanel?.SetActive(false);
        gameWinPanel?.SetActive(false);
        Time.timeScale = 1f;
    }

    // ─────────────────────────────────────────────
    // REGISTRO DE ÁREAS
    // Cada PollutedArea se registra no Start
    // ─────────────────────────────────────────────

    public void RegisterArea()
    {
        _totalAreas++;
    }

    public void OnAreaCleared()
    {
        _clearedAreas++;

        if (_clearedAreas >= _totalAreas)
            TriggerGameWin();
    }

    // ─────────────────────────────────────────────
    // GAME OVER
    // ─────────────────────────────────────────────

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel?.SetActive(true);
    }

    // Botão "Tentar Novamente" no painel de game over
    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ─────────────────────────────────────────────
    // GAME WIN
    // ─────────────────────────────────────────────

    private void TriggerGameWin()
    {
        Time.timeScale = 0f;
        gameWinPanel?.SetActive(true);
    }

    // Botão "Menu" no painel de game win
    public void OnMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // cena 0 = menu principal
    }
}