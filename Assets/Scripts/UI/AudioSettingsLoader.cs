using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class AudioSettingsLoader: MonoBehaviour
{
    private EventInstance musicInstance;
    private Bus musicBus;

    private void Awake()
    {
        // Obtén el Bus de música
        musicBus = RuntimeManager.GetBus("bus:/Master/Music");
    }

    private void Start()
    {
        StartCoroutine(StartMusicNextFrame());
        StartCoroutine(ApplySavedBusVolumeNextFrame());
    }

    private IEnumerator StartMusicNextFrame()
    {
        yield return null;

        // Cambia la ruta a tu evento real de música
        musicInstance = RuntimeManager.CreateInstance("event:/Song_SuperTrack 2");
        musicInstance.start();
    }

    private IEnumerator ApplySavedBusVolumeNextFrame()
    {
        yield return null;

        // Aplica el volumen guardado del Music Bus
        float savedVolume = PlayerPrefs.GetFloat("MusicBus", 1f); 
        if (musicBus.isValid())
        {
            musicBus.setVolume(savedVolume);
            Debug.Log("Bus Music volumen inicial: " + savedVolume);
        }
    }
}


