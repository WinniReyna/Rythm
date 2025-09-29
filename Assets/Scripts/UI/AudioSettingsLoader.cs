using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioSettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        StartCoroutine(ApplySavedVolumesNextFrame());
    }

    private IEnumerator ApplySavedVolumesNextFrame()
    {
        yield return null;

        ApplySavedVolume("MasterVolume");
        ApplySavedVolume("MusicVolume");
        ApplySavedVolume("SFXVolume");
        ApplySavedVolume("VideoVolume");
    }

    private void ApplySavedVolume(string parameter)
    {
        float savedValue = PlayerPrefs.GetFloat(parameter, 1f);
        float dB = Mathf.Log10(Mathf.Clamp(savedValue, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
    }
}
