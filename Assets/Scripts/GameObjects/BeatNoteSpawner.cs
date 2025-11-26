using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class BeatNoteSpawner : MonoBehaviour
{
    public AudioSource audioSource;
    public string beatmapFileName = ""; // archivo en StreamingAssets

    private BeatData beatData;

    [HideInInspector] public double songStartDspTime;
    private float travelTime;

    [Header("Referencia al NoteSpawner principal")]
    public NoteSpawner noteSpawner;     // LE PASAMOS LA LISTA DE NOTES
    public Note notesPrefab;           // solo para obtener speed
    public Transform spawnPoint;
    public Transform hitPoint;


    void Start()
    {
        if (noteSpawner == null)
        {
            Debug.LogError("BeatNoteSpawner necesita referencia a NoteSpawner");
            return;
        }

        LoadBeatmap();

        // calcular travelTime igual que antes
        float distance = Vector3.Distance(spawnPoint.position, hitPoint.position);
        float noteSpeed = notesPrefab.speed;
        travelTime = distance / noteSpeed;

        // convertir beats NoteData con su tiempo exacto
        List<NoteData> convertedNotes = new List<NoteData>();

        convertedNotes.Clear();

        for (int i = 0; i < noteSpawner.notes.Count && i < beatData.beats.Count; i++)
        {
            NoteData nd = noteSpawner.notes[i]; // toma la nota del inspector

            nd.time = (float)beatData.beats[i] - travelTime;

            if (nd.time < 0)
                nd.time = 0;

            convertedNotes.Add(nd);
        }

        // Asignamos la lista al NoteSpawner principal
        noteSpawner.notes = convertedNotes;

        Debug.Log($"BeatNoteSpawner generó {convertedNotes.Count} notas y se las pasó a NoteSpawner.");        
    }

    void LoadBeatmap()
    {
        string path = Path.Combine(Application.streamingAssetsPath, beatmapFileName);

        string json;

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("Beatmap NO encontrado: " + path);
            json = "{\"beats\":[]}";
        }

        beatData = JsonUtility.FromJson<BeatData>(json);
    }
}

[System.Serializable]
public class BeatData
{
    public List<double> beats;
}
