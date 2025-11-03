using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject object1; // Primer objeto a activar
    public GameObject object2; // Segundo objeto a activar

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Teleport")
        {
            // Activar los 2 objetos
            object1.SetActive(true);
            object2.SetActive(false);
        }
    }
}
