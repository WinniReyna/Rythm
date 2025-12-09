using UnityEngine;
using TMPro;
using System.Collections;

public class MessageUI : MonoBehaviour, IMessageDisplay
{
    [SerializeField] private GameObject panel;     
    [SerializeField] private TMP_Text messageText;

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false); 
    }

    public void ShowMessage(string text, float duration = 2f)
    {
        if (panel == null || messageText == null)
            return;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        panel.SetActive(true);         
        messageText.text = text;

        if (duration > 0f)
            hideRoutine = StartCoroutine(HideAfterTime(duration));
    }

    public void HideMessage()
    {
        if (panel != null)
            panel.SetActive(false);   
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
    }

    private IEnumerator HideAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        HideMessage();
    }
}
