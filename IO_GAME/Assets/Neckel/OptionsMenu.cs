using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sfxSlider;
    public Slider musicSlider;

    void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            sfxSlider.value = sfxValue;
            SetSFXVolume(sfxValue);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            float musicValue = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicSlider.value = musicValue;
            SetMusicVolume(musicValue);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}