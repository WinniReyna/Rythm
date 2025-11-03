using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

[System.Serializable]
public class AreaCamera
{
    public string areaTag;       // Tag de la zona
    public List<GameObject> objectsToEnable; // Objetos que se activarán en esta zona
}
