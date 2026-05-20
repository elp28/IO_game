using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sfxSlider;
    public Slider musicSlider;

    private string savePath;

    [System.Serializable]
    private class AudioSettings
    {
        public float sfxVolume = 0.75f;
        public float musicVolume = 0.75f;
    }

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "audiosettings.json");

        AudioSettings settings = LoadSettings();

        if (sfxSlider != null)
        {
            sfxSlider.value = settings.sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            SetSFXVolume(settings.sfxVolume);
        }
        if (musicSlider != null)
        {
            musicSlider.value = settings.musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            SetMusicVolume(settings.musicVolume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        SaveSettings();
    }

    public void SetMusicVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MUSICS", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        SaveSettings();
    }

    private void SaveSettings()
    {
        AudioSettings settings = new AudioSettings
        {
            sfxVolume = sfxSlider != null ? sfxSlider.value : 0.75f,
            musicVolume = musicSlider != null ? musicSlider.value : 0.75f
        };

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(savePath, json);
    }

    private AudioSettings LoadSettings()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<AudioSettings>(json);
        }
        return new AudioSettings();
    }
}