using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public string gameSceneName = "Fase1";

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
    }

    void Update()
    {
        // Só pra debug - vê se o botão ainda existe
        if (playButton == null && Input.anyKeyDown)
            Debug.LogError("PlayButton foi deletado!");
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame chamado");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Opções");
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