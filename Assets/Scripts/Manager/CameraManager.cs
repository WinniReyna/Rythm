using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject object1; 
    public GameObject object2; 

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Teleport")
        {
            object1.SetActive(true);
            object2.SetActive(false);
        }
    }
}
