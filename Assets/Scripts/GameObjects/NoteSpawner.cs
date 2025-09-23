using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs de notas")]
    public GameObject notePrefab;       // notas normales
    public GameObject spaceNotePrefab;  // nota especial (Space)

    [Header("Puntos de aparición")]
    public Transform spawnPointA;
    public Transform spawnPointS;
    public Transform spawnPointD;
    public Transform spawnPointSpace;   // zona especial para Space

    [Header("Lista de notas (nivel)")]
    public List<NoteData> notes = new List<NoteData>();

    private float songTimer;

    void Update()
    {
        songTimer += Time.deltaTime;

        for (int i = 0; i < notes.Count; i++)
        {
            if (songTimer >= notes[i].time)
            {
                SpawnNote(notes[i]);
                notes.RemoveAt(i);
                i--;
            }
        }
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

            // Usamos la clase concreta Note (no la interfaz INote) 
            // porque necesitamos pasar más datos al Initialize
            var note = obj.GetComponent<Note>();
            note?.Initialize(
                data.key,
                data.gridX,
                data.gridY,
                data.paintColor,
                data.allowEmptyPaint
            );
        }
        else
        {
            Debug.LogWarning($"No se pudo spawnear nota {data.key}, spawnPoint o prefab es null.");
        }
    }
}






