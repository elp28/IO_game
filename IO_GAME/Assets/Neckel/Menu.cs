using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public string gameSceneName = "Fase1";
    public GameObject optionsPanel;

    void Start()
    {
        Debug.Log("Menu iniciado");

        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
            Debug.Log("Play button configurado");
        }
        else
            Debug.LogError("Play button é NULL!");

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OpenOptions);
            Debug.Log("Options button configurado");
        }
        else
            Debug.LogError("Options button é NULL!");

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            Debug.Log("Quit button configurado");
        }
        else
            Debug.LogError("Quit button é NULL!");
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame chamado");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Opções");
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}