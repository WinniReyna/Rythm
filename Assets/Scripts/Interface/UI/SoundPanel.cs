using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour, IMenuPanel
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
