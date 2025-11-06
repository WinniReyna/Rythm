using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Posición del jugador
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    // Estado de objetos activos
    public List<string> activeObjects = new List<string>();

    // Estado de puertas
    public List<DoorState> doors = new List<DoorState>();
}

[Serializable]
public class DoorState
{
    public string doorID;
    public bool isLocked;
}
