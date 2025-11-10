using UnityEngine;

public class DiggableSpot : MonoBehaviour, IInteractable
{
    [Header("Opcional: qué ocurre al cavar")]
    public GameObject buriedItemPrefab;
    public bool isDug = false;

    // Implementación obligatoria de IInteractable
    public void Interact()
    {
        Debug.Log("Aquí podrías usar una pala...");
        // No llamamos a Dig() aquí, porque la pala lo hará desde el inventario
    }

    // Método específico para usar con la pala
    public void Dig(float digDuration)
    {
        if (isDug)
        {
            Debug.Log("Ya cavaste aquí.");
            return;
        }

        isDug = true;
        Debug.Log($"Cavando durante {digDuration} segundos...");

        if (buriedItemPrefab != null)
        {
            Instantiate(buriedItemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
}

