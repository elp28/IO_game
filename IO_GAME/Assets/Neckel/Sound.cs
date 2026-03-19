using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound Instance;  // ISSO AQUI É O QUE FALTAVA!

    public AudioSource sfxSource;
    public AudioClip clickSound;

    void Awake()
    {
        transform.parent = null;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SoundManager único criado");
        }
        else
        {
            Debug.Log("SoundManager duplicado destruído");
            Destroy(gameObject);
        }
    }

    public void PlayClick()
    {
        if (sfxSource != null && clickSound != null)
            sfxSource.PlayOneShot(clickSound);
    }
}