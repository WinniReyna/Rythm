using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs de notas")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject spaceNotePrefab;

    [Header("Puntos de aparición")]
    [SerializeField] private Transform spawnPointA;
    [SerializeField] private Transform spawnPointS;
    [SerializeField] private Transform spawnPointD;
    [SerializeField] private Transform spawnPointSpace;

    [Header("Lista de notas (nivel)")]
    public List<NoteData> notes = new List<NoteData>();

    [Header("Dificultad")]
    [SerializeField] private DifficultySettings currentDifficulty;

    private float songTimer;
    private bool gameStarted = false;

    // Lista interna de notas que se van a spawnear según la dificultad
    private List<NoteData> activeNotes = new List<NoteData>();

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

    /// <summary>
    /// Inicia el juego y aplica la dificultad seleccionada
    /// </summary>
    public void StartGame(DifficultySettings difficulty)
    {
        currentDifficulty = difficulty;
        gameStarted = true;
        songTimer = 0f;

        // Crear lista interna según spawnRateMultiplier
        activeNotes.Clear();

        int notesToSpawn = Mathf.CeilToInt(notes.Count * currentDifficulty.spawnRateMultiplier);
        notesToSpawn = Mathf.Clamp(notesToSpawn, 1, notes.Count);

        for (int i = 0; i < notesToSpawn; i++)
        {
            activeNotes.Add(notes[i]);
        }

        Debug.Log($"Juego iniciado con dificultad {difficulty.name}. Notas a spawnear: {activeNotes.Count}");
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
            case NoteKey.Space:
                spawnPoint = spawnPointSpace;
                prefab = spaceNotePrefab;
                break;
        }

        if (spawnPoint != null && prefab != null)
        {
            var obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            var note = obj.GetComponent<Note>();
            note?.Initialize(
                data.key,
                data.gridX,
                data.gridY,
                data.paintColor,
                data.allowEmptyPaint
            );

            // Aplicar velocidad desde la dificultad
            note.speed = currentDifficulty.noteSpeed;
        }
        else
        {
            Debug.LogWarning($"No se pudo spawnear nota {data.key}, spawnPoint o prefab es null.");
        }
    }
}








