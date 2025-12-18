using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PauseAudioManager : MonoBehaviour
{
    [Header("FMOD Events")]
    //[SerializeField] private EventReference pauseUISound;        // Sonido UI
    [SerializeField] private EventReference pauseMenuMusic;      // Música del menú de pausa

    [Header("Music Bus Control")]
    [Range(0f, 1f)]
    [SerializeField] private float pausedVolume = 0.6f;          // Volumen del gameplay mientras está pausado
    private Bus musicBus;
    private float originalVolume;

    private EventInstance pauseMusicInstance;

    private void Awake()
    {
        // Bus de música del gameplay
        musicBus = RuntimeManager.GetBus("bus:/Master/Music");
    }
    private void OnEnable()
    {
        // Reproducir sonido UI
        //RuntimeManager.PlayOneShot(pauseUISound);

        // Bajar música del gameplay
        musicBus.getVolume(out originalVolume);
        musicBus.setVolume(pausedVolume);

        // Reproducir música del menú de pausa
        pauseMusicInstance = RuntimeManager.CreateInstance(pauseMenuMusic);
        pauseMusicInstance.start();
    }

    private void OnDisable()
    {
        // Detener música del menú
        pauseMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        pauseMusicInstance.release();

        // Restaurar volumen del gameplay
        musicBus.setVolume(originalVolume);
    }
}
