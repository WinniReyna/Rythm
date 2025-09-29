using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour, IMenuPanel
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider videoSlider;

    private void Start()
    {

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", GetLinearVolume("MasterVolume"));
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", GetLinearVolume("MusicVolume"));
        effectsSlider.value = PlayerPrefs.GetFloat("SFXVolume", GetLinearVolume("SFXVolume"));
        videoSlider.value = PlayerPrefs.GetFloat("VideoVolume", GetLinearVolume("VideoVolume"));


        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetEffectsVolume(effectsSlider.value);
        SetVideoVolume(videoSlider.value);


        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
        videoSlider.onValueChanged.AddListener(SetVideoVolume);
    }

    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVolume", value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetEffectsVolume(float value)
    {
        SetVolume("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetVideoVolume(float value)
    {
        SetVolume("VideoVolume", value);
        PlayerPrefs.SetFloat("VideoVolume", value);
    }


    private void SetVolume(string parameter, float value)
    {

        float dB = Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
    }

    private float GetLinearVolume(string parameter)
    {
        if (audioMixer.GetFloat(parameter, out float dB))
        {
            return Mathf.Pow(10, dB / 20f);
        }
        return 1f;
    }

    private void OnDisable()
    {

        PlayerPrefs.Save();
    }
}
