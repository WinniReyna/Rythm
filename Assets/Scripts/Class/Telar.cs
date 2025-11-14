using UnityEngine;

public class Telar : MonoBehaviour, IInteractable
{
    public string telarName;

    public void Interact()
    {
        Debug.Log($"Interactuando con el telar: {telarName}");
        // Puedes poner aquí lógica extra 
    }
}
