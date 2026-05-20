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
        

        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
            
        }
        else
            

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OpenOptions);
            
        }
        else
            

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
           
        }
        
    }

    public void PlayGame()
    {
       
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        
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