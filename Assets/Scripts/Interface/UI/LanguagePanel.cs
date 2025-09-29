using UnityEngine;

public class LanguagePanel : MonoBehaviour, IMenuPanel
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
