using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Painéis")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    [Header("Transição — Abrir Painel")]
    [SerializeField] private TransitionSettings openPanelTransition;

    [Header("Transição — Botões")]
    [SerializeField] private TransitionSettings restartTransition;
    [SerializeField] private TransitionSettings menuTransition;
    [SerializeField] private float transitionDelay = 0f;

    private int _totalAreas = 0;
    private int _clearedAreas = 0;

    // Painel pendente para ativar no cut point
    private GameObject _pendingPanel;

    void Awake()
    {
        if (instance == null) instance = this;

        gameOverPanel?.SetActive(false);
        gameWinPanel?.SetActive(false);
    }

    // ─────────────────────────────────────────────
    // REGISTRO DE ÁREAS
    // ─────────────────────────────────────────────

    public void RegisterArea() { _totalAreas++; }

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
        ShowPanelWithTransition(gameOverPanel);
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;

        if (restartTransition != null)
            TransitionManager.Instance().Transition(
                SceneManager.GetActiveScene().buildIndex,
                restartTransition,
                transitionDelay);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ─────────────────────────────────────────────
    // GAME WIN
    // ─────────────────────────────────────────────

    private void TriggerGameWin()
    {
        ShowPanelWithTransition(gameWinPanel);
    }

    public void OnMenuClicked()
    {
        Time.timeScale = 1f;

        if (menuTransition != null)
            TransitionManager.Instance().Transition(
                0,
                menuTransition,
                transitionDelay);
        else
            SceneManager.LoadScene(0);
    }

    // ─────────────────────────────────────────────
    // HELPER
    // ─────────────────────────────────────────────

    private void ShowPanelWithTransition(GameObject panel)
    {
        if (openPanelTransition == null)
        {
            panel.SetActive(true);
            return;
        }

        _pendingPanel = panel;

        TransitionManager manager = TransitionManager.Instance();
        manager.onTransitionCutPointReached += OnCutPointReached;
        manager.Transition(openPanelTransition, transitionDelay);
    }

    private void OnCutPointReached()
    {
        // Desinscreve imediatamente usando método nomeado — funciona corretamente
        TransitionManager.Instance().onTransitionCutPointReached -= OnCutPointReached;

        if (_pendingPanel != null)
        {
            _pendingPanel.SetActive(true);
            _pendingPanel = null;
        }
    }
}