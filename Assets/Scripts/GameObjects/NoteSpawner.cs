using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Localization;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs de notas")]
    [SerializeField] private GameObject notePrefab;

    [Header("Puntos de aparición")]
    [SerializeField] private Transform spawnPointA;
    [SerializeField] private Transform spawnPointS;
    [SerializeField] private Transform spawnPointD;
    [SerializeField] private Transform spawnPointShiftLeft;
    [SerializeField] private Transform spawnPointSpace;

    [Header("Lista de notas (nivel)")]
    public List<NoteData> notes;

    [Header("Slider Hit")]
    public HitSlider hitSlider;
    private Note currentSliderNote;

    [Header("Dificultad")]
    [SerializeField] private DifficultySettings currentDifficulty;

    [Header("Contador de inicio")]
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownTime = 3f;

    private float songTimer;
    private bool gameStarted = false;

    private List<NoteData> activeNotes;
    private List<Note> notesInScene;

    public int ActiveNotesCount => activeNotes.Count;

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


    void Update()
    {
        if (!gameStarted) return;

        songTimer += Time.deltaTime;

        for (int i = 0; i < activeNotes.Count; i++)
        {
            if (songTimer >= activeNotes[i].time)
            {
                SpawnNote(activeNotes[i]);
                activeNotes.RemoveAt(i);
                i--;
            }
        }
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
        gameStarted = true;
        songTimer = 0f;

        activeNotes.Clear();

        int notesToSpawn = Mathf.CeilToInt(notes.Count * currentDifficulty.spawnRateMultiplier);
        notesToSpawn = Mathf.Clamp(notesToSpawn, 1, notes.Count);

        if (currentDifficulty.spawnRateMultiplier >= 1f)
        {
            activeNotes.AddRange(notes);
        }
        else
        {
            int seed = currentDifficulty.name.GetHashCode();
            Random.InitState(seed);

            List<NoteData> shuffled = new List<NoteData>(notes);
            for (int i = 0; i < shuffled.Count; i++)
            {
                int randomIndex = Random.Range(i, shuffled.Count);
                (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
            }

            for (int i = 0; i < notesToSpawn; i++)
                activeNotes.Add(shuffled[i]);
        }

        FindObjectOfType<ScoreManager>()?.SetTotalNotes(activeNotes.Count);
        Debug.Log($"Juego iniciado con dificultad {currentDifficulty.name}. Notas a spawnear: {activeNotes.Count}");

        FindObjectOfType<GameManager>()?.OnGameStarted();
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

        if (spawnPoint == null || notePrefab == null)
        {
            Debug.LogWarning($"No se pudo spawnear nota {data.key}, spawnPoint o prefab es null.");
            return;
        }

        var obj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        var note = obj.GetComponent<Note>();

        note?.Initialize(data.key, data.gridX, data.gridY, data.paintSprite);
        note.speed = currentDifficulty.noteSpeed;
        RegisterSpawnedNote(note);

        if (data.isSlider)
        {
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.enabled = false;

            var collider = obj.GetComponent<CircleCollider2D>();
            if (collider != null) collider.enabled = false;

            if (hitSlider != null)
            {
                currentSliderNote = note;
                hitSlider.Activate();
            }
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

    public void OnSliderCompleted(bool success)
    {
        var scoreManager = FindObjectOfType<ScoreManager>();

        if (success)
        {
            Debug.Log("Sumando todos los puntos pendientes (slider exitoso)");
            scoreManager?.CommitPendingPoints();
        }
        else
        {
            Debug.Log("Falló el slider, puntos pendientes eliminados");
            scoreManager?.ClearPendingPoints();
        }

        // Solo si había una nota activa
        if (currentSliderNote != null)
        {
            currentSliderNote.HitSlider();
            currentSliderNote.PaintGridOnHit();
            currentSliderNote = null;
        }
    }

}
