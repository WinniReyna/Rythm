using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public List<string> activeObjects = new List<string>();

    public List<DoorState> doors = new List<DoorState>();
}

[Serializable]
public class DoorState
{
    public string doorID;
    public bool isLocked;
}
