using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BusController : MonoBehaviour
{
    private Bus bus;

    [SerializeField, Range(0f, 1f)]
    private float busVolume = 1f;

    private void Awake()
    {
        bus = RuntimeManager.GetBus("bus:/Master/Music");
    }

    private void Update()
    {
        if (bus.isValid()) // chequeo por si el bus no se encuentra
        {
            bus.setVolume(busVolume);
        }
    }
}
