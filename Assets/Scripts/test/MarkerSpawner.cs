using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System;

public class MarkerSpawner : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference musicEvent; 

    [Header("Prefab a Instanciar")]
    public GameObject prefab;
    public Transform spawnPoint;

    private EventInstance instance;
    private static ConcurrentQueue<string> markerQueue = new ConcurrentQueue<string>();

    [StructLayout(LayoutKind.Sequential)]
    private struct TimelineMarkerProperties
    {
        public System.IntPtr namePtr;
        public uint position;
    }

    private static FMOD.RESULT MarkerCallback(EVENT_CALLBACK_TYPE type, IntPtr eventPtr, IntPtr paramPtr)
    {
        if (type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            var marker = (TimelineMarkerProperties)Marshal.PtrToStructure(paramPtr, typeof(TimelineMarkerProperties));
            string name = Marshal.PtrToStringAnsi(marker.namePtr);
            markerQueue.Enqueue(name);
        }
        return FMOD.RESULT.OK;
    }

    void Start()
    {
        instance = RuntimeManager.CreateInstance(musicEvent);
        instance.setCallback(MarkerCallback, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        instance.start();
    }

    void Update()
    {
        while (markerQueue.TryDequeue(out string markerName))
        {
            if (markerName == "BeatNote")
            {
                Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    void OnDestroy()
    {
        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }
}

