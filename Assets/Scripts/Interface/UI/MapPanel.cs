using UnityEngine;

public class MapPanel : MonoBehaviour, IMenuPanel
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
