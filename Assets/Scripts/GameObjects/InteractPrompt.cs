using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptUI;

    public void Show()
    {
        if (promptUI != null)
            promptUI.SetActive(true);
    }

    public void Hide()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }
}
