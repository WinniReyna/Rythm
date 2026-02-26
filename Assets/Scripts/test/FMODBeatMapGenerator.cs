using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using SonicBloom.Koreo;

[Serializable]
public class BeatMap
{
    public List<double> beats;
}

public class FMODBeatMapGenerator : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference musicEvent;

    [Header("Koreographer")]
    public Koreography koreography; 
    [EventID]
    public string koreoEventID = "BeatNote";

    [Header("Spawn")]
    public GameObject prefab;
    public Transform spawnPoint;

    private EventInstance instance;
    private List<double> beatTimes = new List<double>();
    private static ConcurrentQueue<double> beatQueue = new ConcurrentQueue<double>();

    void Start()
    {
        if (Koreographer.Instance == null)
            return;
        
        Koreographer.Instance.RegisterForEvents(koreoEventID, OnKoreographerBeat);

        instance = RuntimeManager.CreateInstance(musicEvent);
        instance.start();
    }

    void OnKoreographerBeat(KoreographyEvent evt)
    {
        if (koreography == null || koreography.SourceClip == null)
            return;
        

        int sampleRate = koreography.SourceClip.frequency;
        double timeSeconds = evt.StartSample / (double)sampleRate;
        beatQueue.Enqueue(timeSeconds);
    }

    void Update()
    {
        while (beatQueue.TryDequeue(out double beatTime))
        {
            if (prefab != null && spawnPoint != null)
            {
                GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                Note mover = obj.GetComponent<Note>();
                if (mover != null)
                    mover.speed = 5f; 
            }

            beatTimes.Add(beatTime);
            Debug.Log($"Beat Koreographer @ {beatTime:F6}s");
        }
    }

    void OnDestroy()
    {
        if (Koreographer.Instance != null)
            Koreographer.Instance.UnregisterForAllEvents(this);

        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

        if (beatTimes.Count > 0)
            ExportBeatMap();
    }

    void ExportBeatMap()
    {
        BeatMap map = new BeatMap { beats = beatTimes };
        string json = JsonUtility.ToJson(map, true);
        string path = Path.Combine(Application.dataPath, "pruebaBTM.json");
        File.WriteAllText(path, json);
        Debug.Log("Beatmap generado: " + path);
    }
}