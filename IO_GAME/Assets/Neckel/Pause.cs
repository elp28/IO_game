using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;    // Botão de reiniciar
    public Button menuButton;

    private bool paused = false;

    void Start()
    {
        pausePanel.SetActive(false);

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    void PauseGame()
    {
        paused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;  // Importante: volta o tempo ao normal antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia a cena atual
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}