using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioSettingsLoader : MonoBehaviour
{
    public static AudioSettingsLoader Instance; // Singleton
    private EventInstance musicInstance;
    private Bus musicBus;
    private string currentEventPath = "";
    private Coroutine crossfadeCoroutine;

    [Header("Crossfade Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // Duración del crossfade en segundos

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Bus de música
        musicBus = RuntimeManager.GetBus("bus:/Master/Music");

        // Detectar cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name, true);
        ApplySavedBusVolume();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName, bool instant = false)
    {
        // Si la escena es MinigameScene, detenemos música y salimos
        if (sceneName == "MinigameScene")
        {
            if (musicInstance.isValid())
            {
                if (crossfadeCoroutine != null)
                    StopCoroutine(crossfadeCoroutine);

                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
                musicInstance.clearHandle(); // Limpiar la instancia para evitar referencias inválidas
            }

            currentEventPath = ""; // No hay música
            return;
        }

        // Música por defecto para otras escenas
        string musicEventPath = "event:/Music/Menu";

        switch (sceneName)
        {
            case "Menu":
                musicEventPath = "event:/Music/Menu";
                break;
            case "GameScene":
                musicEventPath = "event:/Music/Level";
                break;
        }

        if (currentEventPath == musicEventPath)
            return;

        // Si hay música sonando, crossfade
        if (musicInstance.isValid())
        {
            if (crossfadeCoroutine != null)
                StopCoroutine(crossfadeCoroutine);

            if (instant)
            {
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
                musicInstance = RuntimeManager.CreateInstance(musicEventPath);
                musicInstance.start();
            }
            else
            {
                crossfadeCoroutine = StartCoroutine(CrossfadeMusic(musicEventPath));
            }
        }
        else
        {
            musicInstance = RuntimeManager.CreateInstance(musicEventPath);
            musicInstance.start();
        }

        currentEventPath = musicEventPath;
    }


    private IEnumerator CrossfadeMusic(string newEventPath)
    {
        EventInstance oldMusic = musicInstance;
        musicInstance = RuntimeManager.CreateInstance(newEventPath);
        musicInstance.start();

        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;

            oldMusic.setVolume(1f - t);
            musicInstance.setVolume(t);

            timer += Time.deltaTime;
            yield return null;
        }

        // Detener solo la música antigua
        oldMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        oldMusic.release();        
    }


    private void ApplySavedBusVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicBus", 1f);
        if (musicBus.isValid())
        {
            musicBus.setVolume(savedVolume);
            Debug.Log("Bus Music volumen inicial: " + savedVolume);
        }
    }
}

