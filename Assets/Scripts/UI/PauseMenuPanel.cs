using UnityEngine;

public class PauseMenuPanel : MonoBehaviour, IMenuPanel
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}


