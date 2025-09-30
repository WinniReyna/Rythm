using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [Header("Escena inicial")]
    [SerializeField] private string firstScene = "MenuScene";

    private void Start()
    {
        // Cargar la primera escena del juego después de PersistentScene
        if (!string.IsNullOrEmpty(firstScene))
        {
            // Usamos Additive para no cerrar PersistentScene
            SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
        }
    }
}
