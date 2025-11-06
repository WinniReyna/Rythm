using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonSaveManager : MonoBehaviour
{
    [Header("Objetos a controlar")]
    [SerializeField] private List<GameObject> objectsToManage;
    [SerializeField] private List<Teleporter> doorsToManage;

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        LoadGameState();
    }

    #region Save
    public void SaveGameState()
    {
        SaveData data = new SaveData();

        // Guardar posición del jugador
        var player = FindPlayer();
        if (player != null)
        {
            data.playerPosX = player.transform.position.x;
            data.playerPosY = player.transform.position.y;
            data.playerPosZ = player.transform.position.z;
        }

        // Guardar objetos activos
        foreach (var obj in objectsToManage)
        {
            if (obj.activeSelf)
                data.activeObjects.Add(obj.name);
        }

        // Guardar estado de puertas
        foreach (var door in doorsToManage)
        {
            data.doors.Add(new DoorState
            {
                doorID = door.DoorID,
                isLocked = door.IsLocked
            });
        }

        // Serializar a JSON
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Juego guardado en JSON: " + saveFilePath);
    }
    #endregion

    #region Load
    public void LoadGameState()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("No se encontró archivo de guardado. Se cargará estado por defecto.");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Restaurar posición del jugador
        var player = FindPlayer();
        if (player != null)
            player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);

        // Restaurar objetos activos
        foreach (var obj in objectsToManage)
            obj.SetActive(data.activeObjects.Contains(obj.name));

        // Restaurar estado de puertas
        foreach (var door in doorsToManage)
        {
            var savedDoor = data.doors.Find(d => d.doorID == door.DoorID);
            if (savedDoor != null)
                door.SetLocked(savedDoor.isLocked); // Necesitamos método público SetLocked
        }

        Debug.Log("Juego cargado desde JSON.");
    }
    #endregion

    private PlayerMovement FindPlayer()
    {
        if (PlayerMovement.Instance != null)
            return PlayerMovement.Instance;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            return playerObj.GetComponent<PlayerMovement>();

        return FindObjectOfType<PlayerMovement>();
    }

    #region Debug / Reset
    public void ResetSave()
    {
        if (File.Exists(saveFilePath))
            File.Delete(saveFilePath);

        Debug.Log("Archivo de guardado eliminado.");
    }
    #endregion
}
