using SonicBloom.Koreo;
using UnityEngine;

public class BeatMapExporterEditor : MonoBehaviour
{
    [EventID]
    public string koreoEventID = "BeatNote";
    public GameObject prefab;
    public Transform spawnPoint;

    private void Awake()
    {
        Koreographer.Instance.RegisterForEvents(koreoEventID, SpawnObject);
    }

    public void SpawnObject(KoreographyEvent evt)
    {
            if (prefab != null && spawnPoint != null)
            {
                GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                Note mover = obj.GetComponent<Note>();
                if (mover != null)
                    mover.speed = 5f;
            }            
        
    }

}
