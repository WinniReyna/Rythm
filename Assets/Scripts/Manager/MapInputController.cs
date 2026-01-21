using UnityEngine;

public class MapInputController : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private MenuManager menuManager;

    private IInputProvider inputProvider;

    private void Awake()
    {
        inputProvider = new KeyboardInputProvider();
    }
    private void Update()
    {
        if (inputProvider.MapPressed())
        {
            if (mapPanel.activeSelf)
                menuManager.CloseCurrentPanel();
            else
                menuManager.OpenPanel(mapPanel);
        }
    }
}
