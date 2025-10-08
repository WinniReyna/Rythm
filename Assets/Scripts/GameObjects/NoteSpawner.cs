using System.Collections.Generic;
using UnityEngine;

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
    public List<NoteData> notes = new List<NoteData>();

    [Header("Slider Hit")]
    public HitSlider hitSlider;
    private Note currentSliderNote;

    [Header("Dificultad")]
    [SerializeField] private DifficultySettings currentDifficulty;

    private float songTimer;
    private bool gameStarted = false;

    // Notas a spawnear según dificultad
    private List<NoteData> activeNotes = new List<NoteData>();

    // Notas activas en escena
    private List<Note> notesInScene = new List<Note>();

    public int ActiveNotesCount => activeNotes.Count;

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

    public void StartGame(DifficultySettings difficulty)
    {
        currentDifficulty = difficulty;
        gameStarted = true;
        songTimer = 0f;

        activeNotes.Clear();

        int notesToSpawn = Mathf.CeilToInt(notes.Count * currentDifficulty.spawnRateMultiplier);
        notesToSpawn = Mathf.Clamp(notesToSpawn, 1, notes.Count);

        if (currentDifficulty.spawnRateMultiplier >= 1f)
        {
            // Hard usar todas las notas
            activeNotes.AddRange(notes);
        }
        else
        {
            // Semilla fija para que siempre sea el mismo subconjunto
            int seed = difficulty.name.GetHashCode();
            Random.InitState(seed);

            List<NoteData> shuffled = new List<NoteData>(notes);
            for (int i = 0; i < shuffled.Count; i++)
            {
                int randomIndex = Random.Range(i, shuffled.Count);
                (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
            }

            for (int i = 0; i < notesToSpawn; i++)
            {
                activeNotes.Add(shuffled[i]);
            }
        }

        FindObjectOfType<ScoreManager>()?.SetTotalNotes(activeNotes.Count);
        Debug.Log($"Juego iniciado con dificultad {difficulty.name}. Notas a spawnear: {activeNotes.Count}");

        FindObjectOfType<GameManager>()?.OnGameStarted();
    }

    void SpawnNote(NoteData data)
    {
        Transform spawnPoint = null;
        GameObject prefab = notePrefab;

        switch (data.key)
        {
            case NoteKey.A: spawnPoint = spawnPointA; break;
            case NoteKey.S: spawnPoint = spawnPointS; break;
            case NoteKey.D: spawnPoint = spawnPointD; break;
            case NoteKey.ShiftLeft: spawnPoint = spawnPointShiftLeft; break;
            case NoteKey.Space: spawnPoint = spawnPointSpace; break;
        }

        if (spawnPoint != null && prefab != null)
        {
            var obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            var note = obj.GetComponent<Note>();

            // Inicializar nota con coordenadas y sprite
            note?.Initialize(
                data.key,
                data.gridX,
                data.gridY,
                data.paintSprite
            );

            note.speed = currentDifficulty.noteSpeed;
            RegisterSpawnedNote(note);

            // Activar slider si es nota tipo slider
            if (data.isSlider && hitSlider != null)
            {
                currentSliderNote = note;   // Guardar referencia a la nota slider
                hitSlider.Activate();        // Activar el slider
            }
        }
        else
        {
            Debug.LogWarning($"No se pudo spawnear nota {data.key}, spawnPoint o prefab es null.");
        }
    }

    public void RegisterSpawnedNote(Note note)
    {
        if (note != null) notesInScene.Add(note);
    }

    public void UnregisterNote(Note note)
    {
        if (note != null) notesInScene.Remove(note);
    }

    public bool AllNotesFinished()
    {
        return activeNotes.Count == 0 && notesInScene.Count == 0;
    }
    public void ActivateHitSlider()
    {
        if (hitSlider != null)
            hitSlider.Activate();
    }

    public void OnSliderCompleted()
    {
        if (currentSliderNote != null)
        {
            currentSliderNote.Hit();
            currentSliderNote.PaintGridOnHit();
            currentSliderNote = null;
        }
    }

}






