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

    private void Start()
    {
        // Obtener la dificultad guardada
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
        foreach (var nd in activeNotes)
        {
            while (AudioSettings.dspTime < nd.spawnDspTime)
                yield return null;

            SpawnNote(nd);
        }

        // Una vez que todas las notas fueron spawneadas, limpiar lista activa
        activeNotes.Clear();
    }

    public void StartGame(DifficultySettings difficulty = null)
    {
        // Si no se pasa dificultad, usar la actual guardada
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

        // Iniciar conteo antes del juego
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
            // Usa LeanLocalization para mostrar el texto traducido
            string goText = LeanLocalization.GetTranslationText("StartGo"); // clave de traducción
            countdownText.text = goText != null ? goText : "GO!";
            yield return new WaitForSeconds(0.7f);
            countdownText.gameObject.SetActive(false);
        }

        BeginGameplay();
    }

    private void BeginGameplay()
    {
        // Obtener BeatNoteSpawner
        beatSpawner = FindObjectOfType<BeatNoteSpawner>();

        // Inicializar variables
        gameStarted = true;
        songTimer = 0f;
        activeNotes.Clear();

        // Usar solo las notas definidas en la lista (del inspector)
        activeNotes.AddRange(notes);

        // Iniciar canción primero
        if (beatSpawner != null)
        {
            double dsp = AudioSettings.dspTime + 0.1;
            beatSpawner.audioSource.PlayScheduled(dsp);
            beatSpawner.songStartDspTime = dsp;
        }

        // Ajustar tiempos de las notas
        float countdownOffset = countdownTime + 0.7f;
        float hitX = 1.39f; // posición del hit

        for (int i = 0; i < activeNotes.Count; i++)
        {
            NoteData nd = activeNotes[i];

            Transform spawnPoint = GetSpawnPoint(nd.key);

            Vector3 hitPos = new Vector3(hitX, spawnPoint.position.y, spawnPoint.position.z);
            float distance = Vector3.Distance(spawnPoint.position, hitPos);

            float noteTravelTime = distance / currentDifficulty.noteSpeed;

            nd.spawnDspTime = beatSpawner.songStartDspTime + nd.time + countdownOffset - noteTravelTime;

            if (nd.spawnDspTime < 0)
                nd.spawnDspTime = 0;
        }
        
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnNotesCoroutine());


        // Actualizar ScoreManager
        FindObjectOfType<ScoreManager>()?.SetTotalNotes(activeNotes.Count);
        Debug.Log($"Juego iniciado con {activeNotes.Count} notas definidas en la lista.");

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
            _ => spawnPointA // fallback por si acaso
        };
    }
    void SpawnNote(NoteData data)
    {
        Transform spawnPoint = data.key switch
        {
            NoteKey.A => spawnPointA,
            NoteKey.S => spawnPointS,
            NoteKey.D => spawnPointD,
            NoteKey.Shift => spawnPointShiftLeft,
            NoteKey.Space => spawnPointSpace,
            _ => null
        };

        GameObject prefab = data.key switch
        {
            NoteKey.A => prefabA,
            NoteKey.S => prefabS,
            NoteKey.D => prefabD,
            NoteKey.Shift => prefabShift,
            NoteKey.Space => prefabSpace,
            _ => null
        };

        if (spawnPoint == null || prefab == null)
        {
            Debug.LogWarning($"No se pudo spawnear nota {data.key}, spawnPoint o prefab es null.");
            return;
        }

        var obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        var note = obj.GetComponent<Note>();
        note?.Initialize(data.key, data.gridX, data.gridY, data.paintSprite);
        note.speed = currentDifficulty.noteSpeed;

        // Primero inicializar posición y DSP
        note.InitializeMovement(data.spawnDspTime, new Vector3(1.39f, spawnPoint.position.y, spawnPoint.position.z));

        // Luego iniciar el movimiento
        note.StartMovement();

        RegisterSpawnedNote(note);

        if (data.isSlider)
        {
            currentSliderNote = note;
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            var scoreManager = FindObjectOfType<ScoreManager>();

            if (spriteRenderer != null) spriteRenderer.enabled = false;

            var collider = obj.GetComponent<CircleCollider2D>();
            if (collider != null) collider.enabled = false;            

            // Obtener total de notas spawneadas antes del slider
            int totalNotesBeforeSlider = GetActiveNotesCount();

            // Calcular promedio de hits de la tanda
            scoreManager.CalculateHitPercentages(totalNotesBeforeSlider);

            // Reiniciar contadores para la próxima tanda
            scoreManager.ResetHitCounts();

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

        // Verificar si todas las notas terminaron
        if (AllNotesFinished())
        {
            FindObjectOfType<GameManager>()?.ShowResultsPanel();
        }
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
            notesDestroyed = 0;
        }

        // Solo si había una nota activa
        if (currentSliderNote != null)
        {
            currentSliderNote.HitSlider();
            currentSliderNote = null;
        }
    }

}
