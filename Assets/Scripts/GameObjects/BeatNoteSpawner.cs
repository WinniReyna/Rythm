using UnityEngine;
using System.Collections.Generic;
using System.IO;
using FMODUnity;
using FMOD.Studio;

public class BeatNoteSpawner : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference musicEvent; 
    private EventInstance musicInstance;

    public string beatmapFileName = ""; 

    public BeatData beatData;

    [HideInInspector] public double songStartDspTime;
    private float travelTime;

    [Header("Referencia al NoteSpawner principal")]
    public NoteSpawner noteSpawner;
    public Note notesPrefab;
    public Transform spawnPoint;
    public Transform hitPoint;

    public EventInstance MusicInstance
    {
        get { return musicInstance; }
    }


    void Start()
    {
        if (noteSpawner == null)
        {
            Debug.LogError("BeatNoteSpawner necesita referencia a NoteSpawner");
            return;
        }

        LoadBeatmap();

        float distance = Vector3.Distance(spawnPoint.position, hitPoint.position);
        float noteSpeed = notesPrefab.speed;
        travelTime = distance / noteSpeed;

        List<NoteData> convertedNotes = new List<NoteData>();

        for (int i = 0; i < noteSpawner.notes.Count && i < beatData.beats.Count; i++)
        {
            NoteData nd = noteSpawner.notes[i];

            nd.time = (float)beatData.beats[i] - travelTime;
            if (nd.time < 0)
                nd.time = 0;

            convertedNotes.Add(nd);
        }

        noteSpawner.notes = convertedNotes;
        Debug.Log($"BeatNoteSpawner generó {convertedNotes.Count} notas y se las pasó a NoteSpawner.");
    }

    void LoadBeatmap()
    {
        string path = Path.Combine(Application.streamingAssetsPath, beatmapFileName);
        string json;

        if (File.Exists(path))
            json = File.ReadAllText(path);
        else
        {
            Debug.LogError("Beatmap NO encontrado: " + path);
            json = "{\"beats\":[]}";
        }

        beatData = JsonUtility.FromJson<BeatData>(json);
    }

    public void PlayMusic()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        double dspTime = AudioSettings.dspTime;
        musicInstance.start(); 
        songStartDspTime = dspTime;
    }

    public void StopMusic()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
    }
}

[System.Serializable]
public class BeatData
{
    public List<double> beats;
}

