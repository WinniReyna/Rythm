using UnityEngine;
using System.Collections;

public class MusicStarter : MonoBehaviour
{
    [Header("Config")]
    public AudioSource musicSource; // Arrastra aquí el AudioSource
    public float delay = 6f;        // Exactamente 3 segundos después de Start()

    void Start()
    {
        if (musicSource == null)
        {
            Debug.LogError("MusicStarter: musicSource no asignado.");
            return;
        }
        StartCoroutine(StartMusicAfterDelay());
    }

    IEnumerator StartMusicAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        musicSource.Play();
    }
}
