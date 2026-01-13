using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Localization;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs de notas por tipo")]
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabS;
    [SerializeField] private GameObject prefabD;
    [SerializeField] private GameObject prefabShift;
    [SerializeField] private GameObject prefabSpace;

    [Header("Puntos de aparición")]
    [SerializeField] private Transform spawnPointA;
    [SerializeField] private Transform spawnPointS;
    [SerializeField] private Transform spawnPointD;
    [SerializeField] private Transform spawnPointShiftLeft;
    [SerializeField] private Transform spawnPointSpace;

    [Header("Lista de notas (nivel)")]
    public List<NoteData> notes;

    [Header("Slider Hit")]
    [SerializeField] private HitSlider hitSlider;
    private Note currentSliderNote;

    [Header("Dificultad")]
    [SerializeField] private DifficultySettings currentDifficulty;

    [Header("Contador de inicio")]
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownTime = 3f;

    private float songTimer;
    private bool gameStarted = false;

    private List<NoteData> activeNotes = new List<NoteData>();
    private List<Note> notesInScene = new List<Note>();

    public int ActiveNotesCount => activeNotes.Count;

    [HideInInspector] private int totalNotes = 0;
    [HideInInspector] public int notesDestroyed = 0;

    private BeatNoteSpawner beatSpawner;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private void Start()
    {
        DifficultySettings difficulty = DifficultyManager.Instance?.CurrentDifficulty;

        if (difficulty == null)
        {
            Debug.LogWarning("No se encontró dificultad guardada, usando default");
            difficulty = ScriptableObject.CreateInstance<DifficultySettings>();
        }

        StartGame(difficulty);
    }

    private IEnumerator SpawnNotesCoroutine()
    {
        if (activeNotes.Count == 0)
            Debug.LogWarning("SpawnNotesCoroutine: activeNotes está vacío!");

        foreach (var nd in activeNotes)
        {
            Debug.Log($"Nota {nd.key} programada para dspTime={nd.spawnDspTime}, AudioSettings.dspTime={AudioSettings.dspTime}");

            while (GetMusicTime() < nd.spawnDspTime)
                yield return null;

            SpawnNote(nd);
        }

        Debug.Log("Todas las notas procesadas por SpawnNotesCoroutine");
        activeNotes.Clear();
    }

    public void StartGame(DifficultySettings difficulty = null)
    {
        if (difficulty == null)
        {
            difficulty = DifficultyManager.Instance?.CurrentDifficulty;

            if (difficulty == null)
            {
                Debug.LogWarning("No se encontró dificultad en DifficultyManager. Se usará configuración por defecto.");
                difficulty = ScriptableObject.CreateInstance<DifficultySettings>();
            }
        }

        currentDifficulty = difficulty;
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        float timer = countdownTime;

        while (timer > 0)
        {
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(timer).ToString();

            yield return new WaitForSeconds(1f);
            timer--;
        }

        if (countdownText != null)
        {
            if (countdownText != null)
            {
                string goText = LeanLocalization.GetTranslationText("StartGo");
                countdownText.text = goText != null ? goText : "GO!";
                yield return new WaitForSeconds(0.7f);
                countdownText.gameObject.SetActive(false);
            }
            BeginGameplay();
        }
        
    }


    private void BeginGameplay()
    {
        // Obtener BeatNoteSpawner
        beatSpawner = FindObjectOfType<BeatNoteSpawner>();

        if (beatSpawner == null)
        {
            Debug.LogError("No se encontró BeatNoteSpawner en la escena.");
            return;
        }

        // Inicializar variables
        gameStarted = true;
        songTimer = 0f;
        activeNotes.Clear();

        // Usar solo las notas definidas en la lista (del inspector)
        activeNotes.AddRange(notes);

        

        float spawnX = -16f;  // tu spawn point
        float hitX = -1.44f;  // hit point fijo
        float travelDistance = Mathf.Abs(hitX - spawnX);
        float travelTime = travelDistance / currentDifficulty.noteSpeed;

        // Calcular spawnDspTime de cada nota
        for (int i = 0; i < activeNotes.Count && i < beatSpawner.beatData.beats.Count; i++)
        {
            NoteData nd = activeNotes[i];
            double beatTime = beatSpawner.beatData.beats[i]; // tiempo de llegada al hit
            nd.time = (float)beatTime;

            // spawnDspTime absoluto usando FMOD + offset
            nd.spawnDspTime = beatSpawner.songStartDspTime + beatTime - travelTime;

            // Asegurarse de que spawnDspTime no sea menor que songStartDspTime
            if (nd.spawnDspTime < beatSpawner.songStartDspTime)
                nd.spawnDspTime = beatSpawner.songStartDspTime;

            Debug.Log($"[DEBUG] Nota {nd.key} spawnDspTime={nd.spawnDspTime}, llegada={nd.time}, travelTime={travelTime}");
        }

        // Detener coroutine anterior si existía
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        // Iniciar coroutine para spawnear notas
        spawnCoroutine = StartCoroutine(SpawnNotesCoroutine());

        // Iniciar canción primero
        beatSpawner.PlayMusic(); 

        // Actualizar ScoreManager
        FindObjectOfType<ScoreManager>()?.SetTotalNotes(activeNotes.Count);
        Debug.Log($"Juego iniciado con {activeNotes.Count} notas definidas en la lista.");

        // Notificar a GameManager que el juego comenzó
        FindObjectOfType<GameManager>()?.OnGameStarted();
    }



    private Transform GetSpawnPoint(NoteKey key)
    {
        return key switch
        {
            NoteKey.A => spawnPointA,
            NoteKey.S => spawnPointS,
            NoteKey.D => spawnPointD,
            NoteKey.Shift => spawnPointShiftLeft,
            NoteKey.Space => spawnPointSpace,
            _ => spawnPointA
        };
    }

    void SpawnNote(NoteData data)
    {
        // Obtener spawn point según la tecla
        Transform spawnPoint = GetSpawnPoint(data.key);
        GameObject prefab = data.key switch
        {
            NoteKey.A => prefabA,
            NoteKey.S => prefabS,
            NoteKey.D => prefabD,
            NoteKey.Shift => prefabShift,
            NoteKey.Space => prefabSpace,
            _ => null
        };

        // Debug adicional
        Debug.Log($"spawnPoint={spawnPoint}, prefab={prefab}");

        if (spawnPoint == null)
        {
            Debug.LogWarning($"SpawnNote: spawnPoint es null para {data.key}");
            return;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"SpawnNote: prefab es null para {data.key}");
            return;
        }

        // Instanciar la nota
        var obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        var note = obj.GetComponent<Note>();
        if (note == null)
        {
            Debug.LogWarning($"SpawnNote: prefab {prefab.name} no tiene componente Note!");
            return;
        }

        // Inicializar nota
        note.Initialize(data.key, data.gridX, data.gridY, data.paintSprite);
        note.speed = currentDifficulty.noteSpeed;

        // Calcular posición del hit point (puedes ajustarlo según tu layout)
        Vector3 hitPosition = new Vector3(1.39f, spawnPoint.position.y, spawnPoint.position.z);

        // Inicializar movimiento sincronizado con FMOD
        note.InitializeMovement(spawnPoint.position, hitPosition, data.spawnDspTime);
        note.StartMovement();

        var hitZones = FindObjectsOfType<HitZone>();
        foreach (var hz in hitZones)
        {
            if (hz.keyToHandle == data.key) // Compara la tecla de la nota con el carril
                hz.SubscribeNote(note);
        }

        // Registrar nota para control de destrucción
        RegisterSpawnedNote(note);

        // Si la nota es un slider
        if (data.isSlider)
        {
            currentSliderNote = note;

            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.enabled = false;

            var collider = obj.GetComponent<CircleCollider2D>();
            if (collider != null) collider.enabled = false;

            int totalNotesBeforeSlider = GetActiveNotesCount();
            FindObjectOfType<ScoreManager>()?.CalculateHitPercentages(totalNotesBeforeSlider);
            FindObjectOfType<ScoreManager>()?.ResetHitCounts();
            hitSlider.Activate();
        }
    }


    public void RegisterSpawnedNote(Note note)
    {
        if (note != null) notesInScene.Add(note);
    }

    public void UnregisterNote(Note note)
    {
        if (note != null) notesInScene.Remove(note);

        if (AllNotesFinished())
            FindObjectOfType<GameManager>()?.ShowResultsPanel();
    }

    public bool AllNotesFinished()
    {
        return activeNotes.Count == 0 && notesInScene.Count == 0;
    }

    public int GetActiveNotesCount()
    {
        totalNotes = notesDestroyed;
        return totalNotes;
    }

    public void OnSliderCompleted(bool success)
    {
        var scoreManager = FindObjectOfType<ScoreManager>();
        string hitType = scoreManager.GetMostFrequentHitBeforeSlider();

        if (success)
        {
            Debug.Log("Sumando todos los puntos pendientes (slider exitoso)");
            currentSliderNote.PaintGridOnHit(hitType);
            scoreManager?.CommitPendingPoints();
            notesDestroyed = 0;
        }
        else
        {
            Debug.Log("Falló el slider, puntos pendientes eliminados");
            scoreManager?.ClearPendingPoints();
            FindObjectOfType<GameManager>()?.RegisterFailedSlider(); 
            notesDestroyed = 0;
        }


        if (currentSliderNote != null)
        {
            currentSliderNote.HitSlider();
            currentSliderNote = null;
        }
    }

    public double GetMusicTime()
    {
        if (beatSpawner == null || !beatSpawner.MusicInstance.isValid())
            return AudioSettings.dspTime; // fallback

        beatSpawner.MusicInstance.getTimelinePosition(out int ms);
        return ms / 1000.0; // convertir ms a segundos
    }

}

