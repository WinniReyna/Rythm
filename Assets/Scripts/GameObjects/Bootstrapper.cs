using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [Header("Escena inicial")]
    [SerializeField] private string firstScene = "MenuScene";

    private void Start()
    {
        if (!string.IsNullOrEmpty(firstScene))        
            SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
        
    }
}
